﻿using System;

namespace Fralle.Core
{
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
	public class ReadOnlyAttribute : MetaAttribute
	{

	}
}
