using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AElf.Contracts.Delegator;
using AElf.EventHandler;
using AElf.Types;
using Google.Protobuf;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Tank.Contracts.Financing;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Tank.Financing.Permissions;

namespace Tank.Financing.Applies
{

    [Authorize(FinancingPermissions.Applies.Default)]
    public class AppliesAppService : ApplicationService, IAppliesAppService
    {
        public ILogger<AppliesAppService> Logger { get; set; }

        private readonly IApplyRepository _applyRepository;

        private readonly NodeManager _nodeManager = new(FinancingConsts.DefaultNodeUrl,
            FinancingConsts.DefaultSenderAddress,
            FinancingConsts.DefaultSenderPassword);

        public AppliesAppService(IApplyRepository applyRepository)
        {
            _applyRepository = applyRepository;
            Logger = NullLogger<AppliesAppService>.Instance;
        }

        public virtual async Task<PagedResultDto<ApplyDto>> GetListAsync(GetAppliesInput input)
        {
            var totalCount = await _applyRepository.GetCountAsync(input.FilterText, input.EnterpriseName, input.Organization, input.ProductName, input.Allowance, input.APR, input.Period, input.ApplyStatus, input.GuaranteeMethod, input.ApplyTimeMin, input.ApplyTimeMax, input.PassedTimeMin, input.PassedTimeMax);
            var items = await _applyRepository.GetListAsync(input.FilterText, input.EnterpriseName, input.Organization, input.ProductName, input.Allowance, input.APR, input.Period, input.ApplyStatus, input.GuaranteeMethod, input.ApplyTimeMin, input.ApplyTimeMax, input.PassedTimeMin, input.PassedTimeMax, input.Sorting, input.MaxResultCount, input.SkipCount);

            return new PagedResultDto<ApplyDto>
            {
                TotalCount = totalCount,
                Items = ObjectMapper.Map<List<Apply>, List<ApplyDto>>(items)
            };
        }

        public virtual async Task<ApplyDto> GetAsync(Guid id)
        {
            return ObjectMapper.Map<Apply, ApplyDto>(await _applyRepository.GetAsync(id));
        }

        [Authorize(FinancingPermissions.Applies.Delete)]
        public virtual async Task DeleteAsync(Guid id)
        {
            await _applyRepository.DeleteAsync(id);
        }

        [Authorize(FinancingPermissions.Applies.Create)]
        public virtual async Task<ApplyDto> CreateAsync(ApplyCreateDto input)
        {
            Apply(input);
            if (!string.IsNullOrEmpty(input.Allowance))
            {
                AdvanceSetAllowance(input);
            }

            var apply = ObjectMapper.Map<ApplyCreateDto, Apply>(input);
            apply = await _applyRepository.InsertAsync(apply, autoSave: true);
            return ObjectMapper.Map<Apply, ApplyDto>(apply);
        }

        private void Apply(ApplyCreateDto input)
        {
            ForwardContract(input.EnterpriseName, FinancingConsts.ScopeIdForEnterprise, nameof(Apply),
                new ApplyInput
                {
                    EnterpriseName = input.EnterpriseName,
                    ProductName = input.ProductName,
                    Organization = input.Organization
                });
        }
        
        private void AdvanceSetAllowance(ApplyCreateDto input)
        {
            ForwardContract(input.Organization, FinancingConsts.ScopeIdForFinancingOrganization, nameof(SetAllowance),
                new SetAllowanceInput
                {
                    EnterpriseName = input.EnterpriseName,
                    ProductName = input.ProductName,
                    Organization = input.Organization,
                    Allowance = input.Allowance,
                    Apr = input.APR,
                    GuaranteeMethod = input.GuaranteeMethod.ToString(),
                    Period = input.Period
                });
        }

        [Authorize(FinancingPermissions.Applies.Edit)]
        public virtual async Task<ApplyDto> UpdateAsync(Guid id, ApplyUpdateDto input)
        {
            if (input.ApplyStatus == ApplyStatus.线上初审通过)
            {
                OnlineApprove(input);
            }

            if (input.ApplyStatus == ApplyStatus.线下审核通过)
            {
                OfflineApprove(input);
            }

            if (input.ApplyStatus == ApplyStatus.完成)
            {
                ApproveAllowance(input);
            }

            var apply = await _applyRepository.GetAsync(id);
            ObjectMapper.Map(input, apply);
            apply = await _applyRepository.UpdateAsync(apply, autoSave: true);
            return ObjectMapper.Map<Apply, ApplyDto>(apply);
        }

        private void SetAllowance(ApplyUpdateDto input)
        {
            ForwardContract(input.Organization, FinancingConsts.ScopeIdForFinancingOrganization, nameof(SetAllowance),
                new SetAllowanceInput
                {
                    EnterpriseName = input.EnterpriseName,
                    ProductName = input.ProductName,
                    Organization = input.Organization,
                    Allowance = input.Allowance,
                    Apr = input.APR,
                    GuaranteeMethod = input.GuaranteeMethod.ToString(),
                    Period = input.Period
                });
        }

        private void OnlineApprove(ApplyUpdateDto input)
        {
            ForwardContract(input.Organization, FinancingConsts.ScopeIdForFinancingOrganization, nameof(OnlineApprove),
                new ApproveInput
                {
                    EnterpriseName = input.EnterpriseName,
                    ProductName = input.ProductName,
                    Organization = input.Organization
                });
        }
        
        private void OfflineApprove(ApplyUpdateDto input)
        {
            ForwardContract(input.Organization, FinancingConsts.ScopeIdForFinancingOrganization, nameof(OfflineApprove),
                new ApproveInput
                {
                    EnterpriseName = input.EnterpriseName,
                    ProductName = input.ProductName,
                    Organization = input.Organization
                });
        }
        
        private void ApproveAllowance(ApplyUpdateDto input)
        {
            ForwardContract(input.Organization, FinancingConsts.ScopeIdForFinancingOrganization, nameof(ApproveAllowance),
                new ApproveAllowanceInput
                {
                    EnterpriseName = input.EnterpriseName,
                    ProductName = input.ProductName,
                    Organization = input.Organization,
                    Allowance = input.Allowance,
                    Apr = input.APR,
                    GuaranteeMethod = input.GuaranteeMethod.ToString(),
                    Period = input.Period
                });
        }

        private void ForwardContract(string fromId, string scopeId, string methodName,
            IMessage param)
        {
            var txId = _nodeManager.SendTransaction(FinancingConsts.DefaultSenderAddress,
                FinancingConsts.DefaultDelegatorContractAddress,
                "Forward",
                new ForwardInput
                {
                    FromId = fromId,
                    ToAddress = Address.FromBase58(FinancingConsts.DefaultFinancingContractAddress),
                    MethodName = methodName,
                    Parameter = param.ToByteString(),
                    ScopeId = scopeId
                });
            var result = _nodeManager.CheckTransactionResult(txId);
            Logger.LogWarning($"{methodName} tx sent: {result.TransactionId}");
            if (result.Status != "MINED")
            {
                throw new TransactionFailedException($"Transaction execution failed: {result.Error}");
            }
        }
    }
}