using System;
using System.Diagnostics;
using System.Reflection;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using ClientCW.ViewModels;

namespace ClientCW.Views;

public partial class Order : UserControl
{
   
    public Order()
    {
        InitializeComponent();
        ComboBox_Type.SelectedIndex = 0;
    }

    private void InputCustomer(object? sender, Avalonia.Input.KeyEventArgs e)
    {
        NoRU_Input(sender, e);        
    }

    private void InputComment(object? sender, Avalonia.Input.KeyEventArgs e)
    {
        NoRU_Input(sender, e);
    }

    private void InputDruft(object? sender, Avalonia.Input.KeyEventArgs e)
    {
        OnlyNumber_Input(sender, e);
    }

    private void InputWeight(object? sender, Avalonia.Input.KeyEventArgs e)
    {
        OnlyNumber_Input(sender, e);
    }

    private void InputId(object? sender, Avalonia.Input.KeyEventArgs e)
    {
        InputId_Input(sender, e);
    }

    private void InputId_TextChanged(object? sender, Avalonia.Controls.TextChangedEventArgs e)
    {
        ToUpper_Input(sender);
    }



    private void ToUpper_Input(object? sender)
    {
        if (sender != null)
        {
            TextBox textBox = (TextBox)sender;
            textBox.Text = textBox.Text.ToUpper();
        }
        
    }

    private void InputId_Input(object? sender, Avalonia.Input.KeyEventArgs e)
    {
        if (e.KeySymbol != null)
        {
            char ch = Char.Parse(e.KeySymbol);
            if ((ch >= 'a' && ch <= 'z') || (ch >= 'A' && ch <= 'Z') || (ch >= '0' && ch <= '9') || (ch == 8) || (ch == '-') || (ch == '.'))
            {
                e.Handled = false;              // Разрешить                
            }
            else
            {
                e.Handled = true;           // Блокируем
            }           
        }
        else
        {
            e.Handled = true;           // Блокируем
        }
    }

    private void NoRU_Input(object? sender, Avalonia.Input.KeyEventArgs e)
    {
        if (e.KeySymbol != null)
        {
            int code = Char.Parse(e.KeySymbol);           
            if (code > 127)
            {               
                e.Handled = true;  // Блокируем
            }          
        }
    }

    private void OnlyNumber_Input(object? sender, Avalonia.Input.KeyEventArgs e)
    {        
        if (e.KeySymbol != null)
        {
            int code = Char.Parse(e.KeySymbol);            
            if (code != 127 && (code < 48 || code > 57 ))
            {
                e.Handled = true;  // Блокируем
            }
        }        
    }

    
    private void ComboBox_SelectionChanged(object? sender, Avalonia.Controls.SelectionChangedEventArgs e)
    {
        ComboBox comboBox = (ComboBox)sender;
        int index = comboBox.SelectedIndex;
        //Debug.WriteLine("index = " + index);
        if (OrderWeightInput == null) return;

        switch (index)
        {
            case 0:
                OrderWeightInput.IsEnabled = false;
                OrderWeightInput.Text = "0";
                break;
            case 1:
                OrderWeightInput.IsEnabled = true;
                OrderWeightInput.Text = "50000000";
                break;

            default:
                break;
        }
    }

    private void TextBox_TextChanging(object? sender, Avalonia.Controls.TextChangingEventArgs e)
    {
        TextBox comboBox = (TextBox)sender;        
        Debug.WriteLine(comboBox.Text);
        e.Handled = false;
    }
}