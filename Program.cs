using Lab2.Data;
using Lab2.Views.Shared;
using Lab2.Views;
using Lab2.Models;
using Lab2;
using Lab2.Services;
using Lab2.Utils;

using System;

public class Program
{
    public static void Main(string[] args)
    {
        DatabaseInitializer databaseInitializer = new();
        databaseInitializer.Initialize();
        Layout layout = new Layout();
        layout.Render();
    }
} 
     

