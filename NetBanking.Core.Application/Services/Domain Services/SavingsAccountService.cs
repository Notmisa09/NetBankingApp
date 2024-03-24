﻿using AutoMapper;
using Microsoft.AspNetCore.Http;
using NetBanking.Core.Application.Helpers;
using NetBanking.Core.Application.Interfaces.Repositories;
using NetBanking.Core.Application.Interfaces.Services;
using NetBanking.Core.Application.Interfaces.Services.Domain_Services;
using NetBanking.Core.Application.ViewModels.SavingsAccount;
using NetBanking.Core.Application.ViewModels.Users;
using NetBanking.Core.Domain.Entities;

namespace NetBanking.Core.Application.Services.Domain_Services
{
    public class SavingsAccountService : GenericService<SaveSavingsAccountViewModel, SavingsAccountViewModel, SavingsAccount>, ISavingsAccountService
    {
        private readonly IMapper _mapper;
        private readonly ISavingsAccountRepository _repository;
        private readonly IAccountService _accountService;

        public SavingsAccountService(
            
            IMapper mapper,
            ISavingsAccountRepository repository,
            IAccountService accountService
            ) : base(repository, mapper)

        {
            _accountService = accountService;
            _mapper = mapper;
            _repository = repository;
        }

        public override async Task<SaveSavingsAccountViewModel> AddAsync(SaveSavingsAccountViewModel vm)
        {
            SavingsAccount entity = _mapper.Map<SavingsAccount>(vm);
            string candidateId = "";
            do
            {
                candidateId = CodeGeneratorHelper.GenerateCode(typeof(SavingsAccount));
            }
            while ((await _repository.FindAllAsync(x => x.Id == candidateId)).Count != 0);
            vm.Id = candidateId;
            entity = await _repository.AddAsync(entity);

            SaveSavingsAccountViewModel svm = _mapper.Map<SaveSavingsAccountViewModel>(entity);
            return svm;
        }

        public async Task<List<SavingsAccountViewModel>> GetByOwnerIdAsync(string Id)
        {
            var list = await _repository.FindAllAsync(x => x.UserId == Id);
            return _mapper.Map<List<SavingsAccountViewModel>>(list);
        }

        public async Task SaveUserWIthAccount(SaveUserViewModel vm)
        {
            string productcode = string.Empty;
            var userinfo = await _accountService.GetByEmail(vm.Email);
            var code = CodeGeneratorHelper.GenerateCode(typeof(CreditCard));

            SavingsAccount savingAccount = new()
            {
                Amount = vm.InitialAmount,
                IsMain = false,
                UserId = userinfo.Id,
                CreatedDate = DateTime.Now,
                CreatedById = "Default",
                Id = code
            };
            await _repository.AddAsync(savingAccount);
        }

        public async Task SaveUserWIthMainAccount(SaveUserViewModel vm)
        {
            string productcode = string.Empty;
            var userinfo = await _accountService.GetByEmail(vm.Email);
            var code = CodeGeneratorHelper.GenerateCode(typeof(CreditCard));

            SavingsAccount savingAccount = new()
            {
                Amount = vm.InitialAmount,
                IsMain = true,
                UserId = userinfo.Id,
                CreatedDate = DateTime.Now,
                CreatedById = "Default",
                Id = code
            };
            await _repository.AddAsync(savingAccount);
        }
        public override async Task<string> Delete(string Id)
        {
            var savingsAccount = await _repository.GeEntityByIDAsync(Id);

            if (savingsAccount.IsMain == true)
            {
                return "La cuenta principal no puede ser eliminada.";
            }

            else if (savingsAccount.IsMain == false && savingsAccount.Amount >= 0)
            {
                var user = await _accountService.GetByIdAsync(savingsAccount.UserId);

                var savingsAccountPrincipal = await GetByOwnerIdAsync(user.Id);
                var savingsAccountVm = savingsAccountPrincipal.Find(x => x.IsMain == true && x.UserId == savingsAccount.Id);

                savingsAccountVm.Amount += savingsAccount.Amount;
                SaveSavingsAccountViewModel savingsAccountRequest = _mapper.Map<SaveSavingsAccountViewModel>(savingsAccountVm);
                await UpdateAsync(savingsAccountRequest, savingsAccountRequest.Id);

                await _repository.DeleteAsync(savingsAccount);

                return "Se ha borrado la cuenta. Se ha promovido el dinero a la cuenta de ahorro principal.";
            }

            await _repository.DeleteAsync(savingsAccount);
            return "Se ha borrado la cuenta";
        }
    }
}
