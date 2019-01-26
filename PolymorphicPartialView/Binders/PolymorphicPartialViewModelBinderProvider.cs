using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PolymorphicPartialView.Binders
{
    public class PolymorphicPartialViewModelBinderProvider : IModelBinderProvider
    {
        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            if (context.Metadata.IsComplexType && !context.Metadata.IsCollectionType && context.Metadata.Properties["PartialView"] != null)
            {
                Dictionary<ModelMetadata, IModelBinder> binderDictionary = new Dictionary<ModelMetadata, IModelBinder>();

                ILoggerFactory loggerFactory = context.Services.GetRequiredService<ILoggerFactory>();
                ApplicationPartManager applicationPartManager = context.Services.GetRequiredService<ApplicationPartManager>();
                IModelMetadataProvider modelMetadataProvider = context.Services.GetRequiredService<IModelMetadataProvider>();

                List<Type> modelTypes = (from modelType in context.Metadata.ModelType.Assembly.GetTypes()
                                    where context.Metadata.ModelType.IsAssignableFrom(modelType)
                                    select modelType).ToList();

                foreach (Type modelType in modelTypes)
                {
                    ModelMetadata modelMetadata = modelMetadataProvider.GetMetadataForType(modelType);

                    for (int i = 0; i < modelMetadata.Properties.Count; i++)
                    {
                        ModelMetadata modelMetadataProperty = modelMetadata.Properties[i];

                        if (!binderDictionary.ContainsKey(modelMetadataProperty))
                        {
                            binderDictionary.Add(modelMetadataProperty, context.CreateBinder(modelMetadataProperty));
                        }
                    }
                }

                return new PolymorphicPartialViewModelBinder(binderDictionary, loggerFactory, applicationPartManager, modelMetadataProvider);
            }

            return null;
        }
    }
}
