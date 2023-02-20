using System.Collections.Generic;
using System;
using System.Linq;

namespace Entities.Entities
{
	public enum IncomeCategory
	{
		Salary = 1,
		Remittance = 2,
		Investment = 3,
		Gift = 4,
		Deposit = 5,
		Other = 6
	}

	public enum ExpensesCategory
	{
		Remittance = 1,
		Investment = 2,
		Gift = 3,
		Transport = 4,
		Leisure = 5,
		SportAndHealth = 6,
		Shopping = 7,
		FoodAnddrinks = 8,
		Education = 9,
		InternetAndCommunication = 10,
		HousingFee = 11,
		UtilityPayments = 12,
		Other = 13
	}
	public class EnumOptions
	{
		public List<string> Enum1Options { get; set; }
		public List<string> Enum2Options { get; set; }
	}
	public class MyEnumIncomes
	{
		public static EnumOptions GetMyExpensesEnum()
		{
			EnumOptions options = new()
			{
				Enum1Options = Enum.GetNames(typeof(IncomeCategory)).ToList(),
				Enum2Options = Enum.GetNames(typeof(ExpensesCategory)).ToList()
			};
			return options;
		}
	}
}
