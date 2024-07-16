using System;
using System.Linq;

namespace NaughtyAttributes
{
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
	public class HideIfAttribute : ShowIfAttributeBase
	{
		public HideIfAttribute(string condition)
			: base(condition)
		{
			Inverted = true;
		}

		public HideIfAttribute(EConditionOperator conditionOperator, params string[] conditions)
			: base(conditionOperator, conditions)
		{
			Inverted = true;
		}

		public HideIfAttribute(string enumName, params object[] enumValue)
			: base(enumName, enumValue.Select(e => e as Enum).ToArray())
		{
			Inverted = true;
		}
	}
}
