using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Microsoft.AspNetCore.Mvc.Razor.Compilation;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace PolymorphicPartialView.Binders
{
    public class PolymorphicPartialViewModelBinder : ComplexTypeModelBinder
    {
        private Func<object> _modelCreator;

        private ApplicationPartManager ApplicationPartManager { get; }
        private IModelMetadataProvider ModelMetadataProvider { get; }

        public PolymorphicPartialViewModelBinder(IDictionary<ModelMetadata, IModelBinder> propertyBinder, ILoggerFactory loggerFactor,
            ApplicationPartManager applicationPartManager,
            IModelMetadataProvider modelMetadataProvider)
            : base(propertyBinder, loggerFactor)
        {
            this.ApplicationPartManager = applicationPartManager;

            this.ModelMetadataProvider = modelMetadataProvider;
        }
        protected override object CreateModel(ModelBindingContext bindingContext)
        {
            var valueProviderResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName + ".PartialView");

            string partialView = valueProviderResult.FirstValue;

            ViewsFeature viewsFeature = new ViewsFeature();

            this.ApplicationPartManager.PopulateFeature(viewsFeature);

            IEnumerable<CompiledViewDescriptor> compiledViewDescriptors = viewsFeature.ViewDescriptors;

            CompiledViewDescriptor compiledViewDescriptor = compiledViewDescriptors.Single(cvd => cvd.RelativePath == partialView);

            Type modelType = compiledViewDescriptor.Type.BaseType.GenericTypeArguments[0];

            bindingContext.ModelMetadata = this.ModelMetadataProvider.GetMetadataForType(modelType);

            this._modelCreator = Expression
                    .Lambda<Func<object>>(Expression.New(modelType))
                    .Compile();

            return this._modelCreator();
        }
    }
}
