using System;
using System.Collections.Generic;
using EFSamurai.DataAccess;
using EFSamurai.Domain;
using EFSamurai.Domain.Entities;
using NUnit.Framework;

namespace EFSamurai.NUnitTest
{
    public class TddUnitTests
	{
		[OneTimeSetUp]
		public void OneTimeSetup()
		{
			EfTddMethods.RebuildDatabase();
		}


		[SetUp]
		public void Setup()
		{
			EfTddMethods.ClearAllData();
		}


		[Test]
		public void Test1_AddThreeSamuraisListNamesOfThoseWithWesternHairstyles()
		{
			EfTddMethods.CreateSamurai("Williams Adams", HairStyle.Western);
			EfTddMethods.CreateSamurai("Tokugawa Leyasu", HairStyle.Oicho);
			EfTddMethods.CreateSamurai("Jan Joosten", HairStyle.Western);
			List<string> result = EfTddMethods.ReadAlphabeticallyAllSamuraiNamesWithSpecificHairstyle(HairStyle.Western);
			CollectionAssert.AreEqual(new List<string>(){ "Jan Joosten", "Williams Adams" }, result);
		}


		[Test]
		public void Test2_AddThreeSamuraisWithQuotesReadCheesyQuotes()
		{
			EfTddMethods.CreateSamurai(new Samurai(){ Name = "Himura Kenshin", Quotes = new List<Quote>(){ new(){ Text = "You can always die. It's living that takes real courage.", Style = QuoteStyle.Awesome }}});
			EfTddMethods.CreateSamurai(new Samurai(){ Name = "Watsuki Nobuhiro", Quotes = new List<Quote>(){ new(){ Text = "New eras don't come about because of swords, they're created by the people who wield them.", Style = QuoteStyle.Cheesy }}});
			EfTddMethods.CreateSamurai(new Samurai(){ Name = "Laini Taylor", Quotes = new List<Quote>(){ new(){ Text = "Be a Samurai. Because you just never know what's behind the freaking sky.", Style = QuoteStyle.Lame }}});
			List<Quote> result = EfTddMethods.ReadAllQuotesWithSpecificQuoteStyle(QuoteStyle.Cheesy);
			Assert.That(result[0].Text, Is.EqualTo("New eras don't come about because of swords, they're created by the people who wield them."));
			Assert.That(result[0].Samurai?.Name, Is.EqualTo("Watsuki Nobuhiro"));
		}


		[Test]
		public void Test3_AddOneBattle()
		{
			int battleId = EfTddMethods.CreateBattle("WW2", true, "Sauerkraut", new DateTime(1939, 10, 10), new DateTime(1945, 10, 10));
			Battle? actualBattle = EfTddMethods.ReadOneBattle(battleId);
			Assert.That(actualBattle?.Name, Is.EqualTo("WW2"));
			Assert.That(actualBattle?.IsBrutal, Is.EqualTo(true));
			Assert.That(actualBattle?.Description, Is.EqualTo("Sauerkraut"));
			Assert.That(actualBattle?.StartDate, Is.EqualTo(new DateTime(1939, 10, 10)));
			Assert.That(actualBattle?.EndDate, Is.EqualTo(new DateTime(1945, 10, 10)));
		}


		[Test]
		public void Test4_AddSecretIdentity()
		{
			int samuraiId = EfTddMethods.CreateSamurai("Papa Smurf", HairStyle.Western);

			EfTddMethods.CreateOrUpdateSecretIdentitySetRealName(samuraiId, "Rollo Tomasi");
			SecretIdentity? result = EfTddMethods.ReadSecretIdentityOfSpecificSamurai(samuraiId);
			Assert.That(result?.Samurai?.Name, Is.EqualTo("Papa Smurf"));
			Assert.That(result?.RealName, Is.EqualTo("Rollo Tomasi"));

			EfTddMethods.CreateOrUpdateSecretIdentitySetRealName(samuraiId, "Tomas");
			result = EfTddMethods.ReadSecretIdentityOfSpecificSamurai(samuraiId);
			Assert.That(result?.RealName, Is.EqualTo("Tomas"));
		}
	}
}