using System;
using System.Linq;

namespace NaughtyAttributes
{
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
	public class ShowIfAttribute : ShowIfAttributeBase
	{
		public ShowIfAttribute(string condition)
			: base(condition)
		{
			Inverted = false;
		}

		public ShowIfAttribute(EConditionOperator conditionOperator, params string[] conditions)
			: base(conditionOperator, conditions)
		{
			Inverted = false;
		}

		public ShowIfAttribute(string enumName, params object[] enumValue)
			: base(enumName, enumValue.Select(e => e as Enum).ToArray())
		{
			Inverted = false;
		}
	}
}
