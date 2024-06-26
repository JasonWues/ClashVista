using System;
using System.Collections.Generic;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Clash_Vista.ViewModels;

namespace Clash_Vista;

public class ViewLocator : IDataTemplate
{

    readonly private static Dictionary<Type, Func<Control>> Registration = new Dictionary<Type, Func<Control>>();

    public Control? Build(object? data)
    {
        if (data is null)
            return null;

        var type = data.GetType();

        if (Registration.TryGetValue(type, out var factory))
        {
            var control = factory();
            control.DataContext = data;
            return control;
        }
        return new TextBlock { Text = "Not Found: " + type };
    }

    public bool Match(object? data)
    {
        return data is ViewModelBase;
    }

    public static void Register<TViewModel, TView>() where TView : Control, new()
    {
        Registration.Add(typeof(TViewModel), () => new TView());
    }

    public static void Register<TViewModel, TView>(Func<TView> factory) where TView : Control, new()
    {
        Registration.Add(typeof(TViewModel), factory);
    }
}