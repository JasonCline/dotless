﻿using System;
using dotless.Exceptions;
using dotless.Infrastructure;
using dotless.Utils;

namespace dotless.Tree
{
  public class Variable : Node
  {
    public string Name { get; set; }

    public Variable(string name)
    {
      Name = name;
    }

    public override Node Evaluate(Env env)
    {
      var variable = env.Frames.SelectFirst(frame => frame.Variable(Name));

      if (variable)
        return variable.Value.Evaluate(env);

      throw new ParsingException("variable " + Name + " is undefined");
    }

  }
}