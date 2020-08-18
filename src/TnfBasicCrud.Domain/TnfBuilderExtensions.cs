using Microsoft.Extensions.DependencyInjection;

namespace TnfBasicCrud.Domain
{
    public static class TnfBuilderExtensions
    {
        public static void UseDomainLocalization(this ITnfBuilder configuration)
        {
            configuration.Localization(localization =>
            {
                // Incluindo o source de localização
                localization.AddJsonEmbeddedLocalizationFile(
                    Constants.LocalizationSourceName,
                    typeof(Constants).Assembly,
                    "TnfBasicCrud.Domain.Localization.SourceFiles");

                // Incluindo suporte as seguintes linguagens
                localization.AddLanguage("pt-BR", "Português", isDefault: true);
                localization.AddLanguage("en", "English");
            });
        }
    }
}
