using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace health.Extensions
{
    public static class HealthStatusExtensions
    {
        public static HtmlString GetCssClassHealthStatus(this IHtmlHelper html, HealthStatus status)
        {
            return status switch
            {
                HealthStatus.Unhealthy => new HtmlString("bg-danger"),
                HealthStatus.Degraded => new HtmlString("bg-warning"),
                HealthStatus.Healthy => new HtmlString("bg-success"),
                _ => new HtmlString(""),
            };
        }

        public static HtmlString GetNameHealthStatus(this IHtmlHelper html, HealthStatus status)
        {
            return status switch
            {
                HealthStatus.Unhealthy => new HtmlString("Не работает"),
                HealthStatus.Degraded => new HtmlString("Медленно"),
                HealthStatus.Healthy => new HtmlString("Работает"),
                _ => new HtmlString(""),
            };
        }

        public static HtmlString GetCssDuration(this IHtmlHelper html, int evaluationTimeOnSeconds, TimeSpan duration)
        {
            TimeSpan evaluationTimeDuration = new TimeSpan(0, 0, 0, 0, evaluationTimeOnSeconds * 2);

            if (duration > evaluationTimeDuration)
                return new HtmlString("bg-warning");

            return new HtmlString("");
        }
    }
}
