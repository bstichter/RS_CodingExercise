﻿using System;

using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

// Lifted nearly intact from http://stackoverflow.com/a/35083653/4048441
namespace FocusOnTheFamily.ReadyToWed.Metrics.WebSite.TagHelpers {
  [HtmlTargetElement("p", Attributes = ForAttributeName)]
  public class DisplayForTagHelper : TagHelper {
    private const string ForAttributeName = "asp-for";

    [HtmlAttributeName(ForAttributeName)]
    public ModelExpression For { get; set; }

    public override void Process(TagHelperContext context, TagHelperOutput output) {
      if (context == null) {
        throw new ArgumentNullException(nameof(context));
      }

      if (output == null) {
        throw new ArgumentNullException(nameof(output));
      }

      var text = For.ModelExplorer.GetSimpleDisplayText();

      output.Content.SetContent(text);
    }
  }
}