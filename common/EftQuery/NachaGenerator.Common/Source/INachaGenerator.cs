using System.Collections.Generic;
using EFTQuery.Common;

namespace EFTQuery.NachaGenerator.Common
{
    public interface INachaGenerator
    {
        INachaGeneratorResult Generate(INachaGeneratorRequest request);
    }
}