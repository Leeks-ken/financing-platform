using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;
using AElf;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Tank.Financing.Permissions;
using Tank.Financing.Enterprises;

namespace Tank.Financing.Enterprises
{

    [Authorize(FinancingPermissions.Enterprises.Default)]
    public class EnterprisesAppService : ApplicationService, IEnterprisesAppService
    {
        private readonly IEnterpriseRepository _enterpriseRepository;
        private readonly IBlockchainAppService _blockchainAppService;

        public EnterprisesAppService(IEnterpriseRepository enterpriseRepository,
            IBlockchainAppService blockchainAppService)
        {
            _enterpriseRepository = enterpriseRepository;
            _blockchainAppService = blockchainAppService;
        }

        public virtual async Task<PagedResultDto<EnterpriseDto>> GetListAsync(GetEnterprisesInput input)
        {
            var totalCount = await _enterpriseRepository.GetCountAsync(input.FilterText, input.EnterpriseName,
                input.ArtificialPerson, input.EstablishedTimeMin, input.EstablishedTimeMax, input.DueTimeMin,
                input.DueTimeMax, input.CreditCode, input.ArtificialPersonId, input.RegisteredCapital,
                input.PhoneNumber, input.CertPhotoPath, input.IdPhotoPath1, input.IdPhotoPath2, input.CertificateStatus,
                input.CertificateTxId, input.ConfirmCertificateTxId);
            var items = await _enterpriseRepository.GetListAsync(input.FilterText, input.EnterpriseName,
                input.ArtificialPerson, input.EstablishedTimeMin, input.EstablishedTimeMax, input.DueTimeMin,
                input.DueTimeMax, input.CreditCode, input.ArtificialPersonId, input.RegisteredCapital,
                input.PhoneNumber, input.CertPhotoPath, input.IdPhotoPath1, input.IdPhotoPath2, input.CertificateStatus,
                input.CertificateTxId, input.ConfirmCertificateTxId, input.Sorting, input.MaxResultCount,
                input.SkipCount);

            return new PagedResultDto<EnterpriseDto>
            {
                TotalCount = totalCount,
                Items = ObjectMapper.Map<List<Enterprise>, List<EnterpriseDto>>(items)
            };
        }

        public virtual async Task<EnterpriseDto> GetAsync(Guid id)
        {
            return ObjectMapper.Map<Enterprise, EnterpriseDto>(await _enterpriseRepository.GetAsync(id));
        }

        [Authorize(FinancingPermissions.Enterprises.Delete)]
        public virtual async Task DeleteAsync(Guid id)
        {
            await _enterpriseRepository.DeleteAsync(id);
        }

        [Authorize(FinancingPermissions.Enterprises.Create)]
        public virtual async Task<EnterpriseDto> CreateAsync(EnterpriseCreateDto input)
        {
            var txId = _blockchainAppService.Certificate(input);
            input.ArtificialPersonId = HashHelper.ComputeFrom(input.ArtificialPersonId).ToHex();
            input.CertificateTxId = txId;
            var enterprise = ObjectMapper.Map<EnterpriseCreateDto, Enterprise>(input);
            enterprise = await _enterpriseRepository.InsertAsync(enterprise, autoSave: true);
            return ObjectMapper.Map<Enterprise, EnterpriseDto>(enterprise);
        }

        [Authorize(FinancingPermissions.Enterprises.Edit)]
        public virtual async Task<EnterpriseDto> UpdateAsync(Guid id, EnterpriseUpdateDto input)
        {
            if (input.CertificateStatus == CertificateStatus.通过)
            {
                var txId = _blockchainAppService.ConfirmCertificate(input);
                input.ConfirmCertificateTxId = txId;
            }

            var enterprise = await _enterpriseRepository.GetAsync(id);
            ObjectMapper.Map(input, enterprise);
            enterprise = await _enterpriseRepository.UpdateAsync(enterprise, autoSave: true);
            return ObjectMapper.Map<Enterprise, EnterpriseDto>(enterprise);
        }
    }
}