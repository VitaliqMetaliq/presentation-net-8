using Mapster;
using TrueCode.Finance.Application.Dto;
using TrueCode.Shared.Contracts.Entities;

namespace TrueCode.Finance.Application.Mapping
{
    internal class MappingRegistrator : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<CurrencyEntity, CurrencyDto>();
        }
    }
}
