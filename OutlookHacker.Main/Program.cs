using System.Reflection;
using System.Diagnostics;
using OutlookHacker.Models;
// See https://aka.ms/new-console-template for more information

using System;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using OutlookHacker.Crawler.CustomBrowser;
using OutlookHacker.Models.ConstStr;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.Extensions;
using OpenQA.Selenium.Support.UI;
using OutlookHacker.ProxyManager;
using TwoCaptcha;
using TwoCaptcha.Captcha;
using OutlookHacker.Main.MailName;

internal class Program
{
    private static async Task Main(string[] args)
    {
        Console.WriteLine("请输入次数");
        var times = int.Parse(s: Console.ReadLine());
        Console.WriteLine("请输入线程");
        var threadCount = int.Parse(s: Console.ReadLine());
        await Run(times, threadCount);
    }
    private static async Task Run(int times, int threadCount)
    {
        for (int i = 1; i <= threadCount; i++)
        {
            new Thread(async () => await SingleChromeThread(times)).Start();
        }
    }
    private static async Task SingleChromeThread(int times)
    {
        var nameGen = new MailNameGen();
        var p = new ProxyProvider();
        while (p.IsIniting)
        {
            await global::System.Console.Out.WriteLineAsync("正在获取代理池...");
            await Task.Delay(3000);
        }
        for (var i = 1; i <= times; i++)
        {
            var proxy = await p.GetProxy();
            var mailName = nameGen.GetRandomName();
            var passWord = nameGen.GetPassword();
            var browser = new Chrome(proxy).Driver;
            browser.Url = ConstUri.TARGET_URI;
            browser.FindElement(By.CssSelector("#signup")).Click();
            browser.FindElement(By.CssSelector("#iSignupAction")).Click();
            browser.FindElement(By.CssSelector("#MemberName")).SendKeys(mailName);
            browser.FindElement(By.CssSelector("#iSignupAction")).Click();
            browser.FindElement(By.CssSelector("#PasswordInput")).SendKeys(passWord);
            browser.FindElement(By.CssSelector("#iSignupAction")).Click();
            browser.FindElement(By.CssSelector("#LastName")).SendKeys(nameGen.GetLastName());
            browser.FindElement(By.CssSelector("#FirstName")).SendKeys(nameGen.GetFirstName());
            browser.FindElement(By.CssSelector("#iSignupAction")).Click();
            var monthSelectorNum = RandomNumberGenerator.GetInt32(1, 13);
            var daySelectorNum = RandomNumberGenerator.GetInt32(1, 31);
            if (monthSelectorNum == 2)
            {
                daySelectorNum = RandomNumberGenerator.GetInt32(1, 29);
            }
            browser.FindElement(By.CssSelector("#BirthYear")).SendKeys(RandomNumberGenerator.GetInt32(1971, 2000).ToString());
            new SelectElement(browser.FindElement(By.CssSelector("#BirthMonth"))).SelectByIndex(monthSelectorNum);
            new SelectElement(browser.FindElement(By.CssSelector("#BirthDay"))).SelectByIndex(daySelectorNum);
            browser.FindElement(By.CssSelector("#iSignupAction")).Click();
            var value = browser.FindElement(By.CssSelector("#FunCaptcha-Token")).GetAttribute("value")!;
            var guidR = new Regex(RegexStr.MATCH_GUID);
            var surlR = new Regex(RegexStr.MATCH_SURL);
            var guid = guidR.Match(value).Groups[0].Value;
            var surl = surlR.Match(value).Groups[0].Value;
            var result = await SolveFunCaptha(guid, surl, browser.Url);
            browser.ExecuteJavaScript($"document.querySelector('#FunCaptcha-Token').value = {result}");
            browser.ExecuteJavaScript("document.querySelector(form).submit()");
        }
    }

    private static async Task<string> SolveFunCaptha(string token, string surl, string url)
    {
        var solver = new TwoCaptcha.TwoCaptcha("067d7a6590f9a877c974ce5ec2908e59");
        var captcha = new FunCaptcha();
        captcha.SetUserAgent("Mozilla/6.0 (Windows NT 6.3; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/104.0.5112.102 Safari/537.36");
        captcha.SetSiteKey(token);
        captcha.SetUrl(url);
        captcha.SetSUrl(surl);
        await solver.Solve(captcha);
        return captcha.Code;
    }
}