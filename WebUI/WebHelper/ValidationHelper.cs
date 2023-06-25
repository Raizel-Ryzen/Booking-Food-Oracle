using System.ComponentModel.DataAnnotations;
using System.Reflection;
using WebUI.Models;

namespace WebUI.WebHelper;

public static class ValidationHelper<T>
{
    public static ValidationModel IsValid(T t)
    {
        // Get all property of T
        var props = t?.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

        if (props == null) return new ValidationModel(true, "Model is valid.");
        
        foreach (var prop in props)
        {
            var attr = prop.GetCustomAttributes<RequiredAttribute>();
            var requiredAttributes = attr.ToList();
            
            if (requiredAttributes.ToList().Count <= 0) continue;
            
            var propName = prop.Name;
            var propValue = prop.GetValue(t);

            if (propValue != null && !string.IsNullOrEmpty(propValue.ToString())) continue;
            
            var errorMessage = requiredAttributes.FirstOrDefault()?.ErrorMessage ?? $"{propName} is required.";
            
            return new ValidationModel(false, string.IsNullOrEmpty(errorMessage) ? $"{propName} is required." : errorMessage);
        }

        return new ValidationModel(true, "Model is valid.");
    }
}