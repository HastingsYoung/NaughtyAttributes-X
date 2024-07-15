using UnityEditor;

namespace NaughtyAttributes.Editor
{
	internal class SavedInt
	{
		private int _value;
		private string _name;

		public int Value
		{
			get
			{
				return _value;
			}
			set
			{
				if (_value == value)
				{
					return;
				}

				_value = value;
				EditorPrefs.SetInt(_name, value);
			}
		}

		public SavedInt(string name, int value)
		{
			_name = name;
			_value = EditorPrefs.GetInt(name, value);
		}
	}
}