using System;
using System.Collections.Generic;
using System.Text;

namespace Riemer
{
  internal class TestAttribute : Attribute
  {
    public string Msg { get; set; }

    public TestAttribute(string msg)
    {
      Msg = msg;
    }
  }
}