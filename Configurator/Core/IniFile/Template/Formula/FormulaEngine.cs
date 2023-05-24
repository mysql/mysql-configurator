/* Copyright (c) 2023, Oracle and/or its affiliates.

 This program is free software; you can redistribute it and/or modify
 it under the terms of the GNU General Public License as published by
 the Free Software Foundation; version 2 of the License.

 This program is distributed in the hope that it will be useful,
 but WITHOUT ANY WARRANTY; without even the implied warranty of
 MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 GNU General Public License for more details.

 You should have received a copy of the GNU General Public License
 along with this program; if not, write to the Free Software
 Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA 02110-1301  USA */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using MySql.Configurator.Core.Enums;

namespace MySql.Configurator.Core.IniFile.Template.Formula
{
  internal class FormulaEngine
  {
    private readonly Dictionary<string, string> _formulaVariables;
    private Stack _ops;
    private Queue _output;

    public FormulaEngine()
    {
      OriginalExpression = string.Empty;
      TransitionExpression = string.Empty;
      PostfixExpression = string.Empty;
      _formulaVariables = new Dictionary<string, string>();
    }

    public string OriginalExpression { get; private set; }

    public string PostfixExpression { get; private set; }

    public string TransitionExpression { get; private set; }

    public void AssignFormulaVariable(string keyName, string value)
    {
      string variableValue;
      _formulaVariables.TryGetValue(keyName, out variableValue);
      if (variableValue != value)
      {
        _formulaVariables[keyName] = value;
      }
    }

    public string Evaluate()
    {
      var result = new Stack();
      var resultToken = new FormulaToken();

      // While there are input tokens left
      foreach (object obj in _output)
      {
        // Read the next token from input.
        var token = (FormulaToken)obj;
        FormulaToken operand1;
        FormulaToken operand2;
        switch (token.TokenValueType)
        {
          case FormulaTokenType.Number:
          case FormulaTokenType.Constant:
          case FormulaTokenType.Variable:
            result.Push(token);
            break;

          case FormulaTokenType.Assignment:
            if (result.Count >= 2)
            {
              operand2 = (FormulaToken)result.Pop();
              operand1 = (FormulaToken)result.Pop();
              if (operand1.TokenValueType == FormulaTokenType.Variable
                  && (operand2.TokenValueType == FormulaTokenType.Number
                      || operand2.TokenValueType == FormulaTokenType.Constant
                      || operand2.TokenValueType == FormulaTokenType.Variable))
              {
                _formulaVariables[operand1.TokenValue] = GetTokenValue(operand2).ToString(CultureInfo.CurrentCulture);
              }
              else
              {
                throw new Exception("Assignment error!");
              }

              result.Push(operand1);
            }
            else
            {
              throw new Exception("Evaluation error!");
            }
            break;

          case FormulaTokenType.Plus:
            if (result.Count >= 2)
            {
              operand2 = (FormulaToken)result.Pop();
              operand1 = (FormulaToken)result.Pop();
              resultToken.TokenValueType = FormulaTokenType.Number;
              resultToken.TokenValue = (GetTokenValue(operand2) + GetTokenValue(operand1)).ToString(CultureInfo.CurrentCulture);
              result.Push(resultToken);
            }
            else
            {
              throw new Exception("Evaluation error!");
            }
            break;

          case FormulaTokenType.Minus:
            if (result.Count >= 2)
            {
              operand2 = (FormulaToken)result.Pop();
              operand1 = (FormulaToken)result.Pop();
              resultToken.TokenValueType = FormulaTokenType.Number;
              resultToken.TokenValue = (GetTokenValue(operand1) - GetTokenValue(operand2)).ToString(CultureInfo.CurrentCulture);
              result.Push(resultToken);
            }
            else
            {
              throw new Exception("Evaluation error!");
            }
            break;

          case FormulaTokenType.Multiply:
            if (result.Count >= 2)
            {
              operand2 = (FormulaToken)result.Pop();
              operand1 = (FormulaToken)result.Pop();
              resultToken.TokenValueType = FormulaTokenType.Number;
              resultToken.TokenValue = (GetTokenValue(operand2) * GetTokenValue(operand1)).ToString(CultureInfo.CurrentCulture);
              result.Push(resultToken);
            }
            else
            {
              throw new Exception("Evaluation error!");
            }
            break;

          case FormulaTokenType.Divide:
            if (result.Count >= 2)
            {
              operand2 = (FormulaToken)result.Pop();
              operand1 = (FormulaToken)result.Pop();
              resultToken.TokenValueType = FormulaTokenType.Number;
              resultToken.TokenValue = (GetTokenValue(operand1) / GetTokenValue(operand2)).ToString(CultureInfo.CurrentCulture);
              result.Push(resultToken);
            }
            else
            {
              throw new Exception("Evaluation error!");
            }
            break;

          case FormulaTokenType.Exponent:
            if (result.Count >= 2)
            {
              operand2 = (FormulaToken)result.Pop();
              operand1 = (FormulaToken)result.Pop();
              resultToken.TokenValueType = FormulaTokenType.Number;
              resultToken.TokenValue = Math.Pow(GetTokenValue(operand1), GetTokenValue(operand2)).ToString(CultureInfo.CurrentCulture);
              result.Push(resultToken);
            }
            else
            {
              throw new Exception("Evaluation error!");
            }
            break;

          case FormulaTokenType.UnaryMinus:
            if (result.Count >= 1)
            {
              operand1 = (FormulaToken)result.Pop();
              resultToken.TokenValueType = FormulaTokenType.Number;
              resultToken.TokenValue = (-GetTokenValue(operand1)).ToString(CultureInfo.CurrentCulture);
              result.Push(resultToken);
            }
            else
            {
              throw new Exception("Evaluation error!");
            }
            break;

          case FormulaTokenType.Sine:
            if (result.Count >= 1)
            {
              operand1 = (FormulaToken)result.Pop();
              resultToken.TokenValueType = FormulaTokenType.Number;
              resultToken.TokenValue = Math.Sin(GetTokenValue(operand1)).ToString(CultureInfo.CurrentCulture);
              result.Push(resultToken);
            }
            else
            {
              throw new Exception("Evaluation error!");
            }
            break;

          case FormulaTokenType.Cosine:
            if (result.Count >= 1)
            {
              operand1 = (FormulaToken)result.Pop();
              resultToken.TokenValueType = FormulaTokenType.Number;
              resultToken.TokenValue = Math.Cos(GetTokenValue(operand1)).ToString(CultureInfo.CurrentCulture);
              result.Push(resultToken);
            }
            else
            {
              throw new Exception("Evaluation error!");
            }
            break;

          case FormulaTokenType.Tangent:
            if (result.Count >= 1)
            {
              operand1 = (FormulaToken)result.Pop();
              resultToken.TokenValueType = FormulaTokenType.Number;
              resultToken.TokenValue = Math.Tan(GetTokenValue(operand1)).ToString(CultureInfo.CurrentCulture);
              result.Push(resultToken);
            }
            else
            {
              throw new Exception("Evaluation error!");
            }
            break;

          case FormulaTokenType.Round:
            if (result.Count >= 2)
            {
              operand2 = (FormulaToken)result.Pop();
              operand1 = (FormulaToken)result.Pop();
              resultToken.TokenValueType = FormulaTokenType.Number;
              resultToken.TokenValue = Math.Round((GetTokenValue(operand1) / GetTokenValue(operand2)) * GetTokenValue(operand2)).ToString(CultureInfo.CurrentCulture);
              result.Push(resultToken);
            }
            else
            {
              throw new Exception("Evaluation error!");
            }
            break;

          case FormulaTokenType.Max:
            if (result.Count >= 2)
            {
              operand2 = (FormulaToken)result.Pop();
              operand1 = (FormulaToken)result.Pop();
              result.Push(GetTokenValue(operand1) >= GetTokenValue(operand2) ? operand1 : operand2);
            }
            else
            {
              throw new Exception("Evaluation error!");
            }
            break;

          case FormulaTokenType.Min:
            if (result.Count >= 2)
            {
              operand2 = (FormulaToken)result.Pop();
              operand1 = (FormulaToken)result.Pop();
              result.Push(GetTokenValue(operand1) <= GetTokenValue(operand2) ? operand1 : operand2);
            }
            else
            {
              throw new Exception("Evaluation error!");
            }
            break;
        }
      }

      switch (result.Count)
      {
        case 0:
          // No value on the stack means there is no value associated with a parameter.
          return "";

        case 1:
          // If there is only one value in the stack, that value is the result of the calculation.
          return GetTokenText((FormulaToken)result.Pop());

        default:
          // If there are more values in the stack
          // (Error) The user input too many values.
          throw new Exception("Evaluation error. Invalid number of arguments.");
      } 
    }

    public void Parse(string expression)
    {
      _output = new Queue();
      _ops = new Stack();

      OriginalExpression = expression;

      string rpnBuffer = expression;

      // filter out the K, M, G unit specifiers.
      rpnBuffer = Regex.Replace(rpnBuffer, @"(?<number>\d+(\.\d+)?)K", " ${number} * 1024 ");
      rpnBuffer = Regex.Replace(rpnBuffer, @"(?<number>\d+(\.\d+)?)M", " ${number} * 1024 * 1024");
      rpnBuffer = Regex.Replace(rpnBuffer, @"(?<number>\d+(\.\d+)?)G", " ${number} * 1024 * 1024 * 1024");
      rpnBuffer = rpnBuffer.ToLower();

      // captures numbers. Anything like 11 or 22.34 is captured
      rpnBuffer = Regex.Replace(rpnBuffer, @"(?<number>\d+(\.\d+)?)", " ${number} ");
      // captures these symbols: + - * / ^ ( )
      rpnBuffer = Regex.Replace(rpnBuffer, @"(?<ops>[+\-*/^:()])", " ${ops} ");
      // captures constants, variables, and math functions.
      rpnBuffer = Regex.Replace(rpnBuffer, @"(?<var>([a-z_]+))", " ${var} ");
      // trims up consecutive spaces and replace it with just one space
      rpnBuffer = Regex.Replace(rpnBuffer, @"\s+", " ").Trim();

      // The following chunk captures unary minus operations.
      // 1) We replace every minus sign with the string "MINUS".
      // 2) Then if we find a "MINUS" with a number or constant in front,
      //    then it's a normal minus operation.
      // 3) Otherwise, it's a unary minus operation.

      // Step 1.
      rpnBuffer = Regex.Replace(rpnBuffer, "-", "MINUS");
      // Step 2. Looking for pi or e or generic number \d+(\.\d+)?
      //rpn_buffer = Regex.Replace(rpn_buffer, @"(?<number>(pi|e|(\d+(\.\d+)?)))\s+MINUS", "${number} -");
      rpnBuffer = Regex.Replace(rpnBuffer, @"(?<number>([a-z_]+|[)]|(\d+(\.\d+)?)))\s+MINUS", "${number} -");
      // Step 3. Use the tilde ~ as the unary minus operator
      rpnBuffer = Regex.Replace(rpnBuffer, "MINUS", "~");

      TransitionExpression = rpnBuffer;

      // tokenize it!
      string[] parsedTokens = rpnBuffer.Split(" ".ToCharArray());
      int i;
      FormulaToken opstoken;
      for (i = 0; i < parsedTokens.Length; ++i)
      {
        var token = new FormulaToken
        {
          TokenValue = parsedTokens[i],
          TokenValueType = FormulaTokenType.None
        };

        double tokenValue;
        if (double.TryParse(parsedTokens[i], out tokenValue))
        {
          token.TokenValueType = FormulaTokenType.Number;
          // If the token is a number, then add it to the output queue.
          _output.Enqueue(token);
          continue;
        }

        switch (parsedTokens[i])
        {
          case "+":
            token.TokenValueType = FormulaTokenType.Plus;
            if (_ops.Count > 0)
            {
              opstoken = (FormulaToken)_ops.Peek();

              // while there is an operator, o2, at the top of the stack
              while (IsOperatorToken(opstoken.TokenValueType))
              {
                // pop o2 off the stack, onto the output queue;
                _output.Enqueue(_ops.Pop());
                if (_ops.Count > 0)
                  opstoken = (FormulaToken)_ops.Peek();
                else
                  break;
              }
            }
            // push o1 onto the operator stack.
            _ops.Push(token);
            break;

          case "-":
            token.TokenValueType = FormulaTokenType.Minus;
            if (_ops.Count > 0)
            {
              opstoken = (FormulaToken)_ops.Peek();

              // while there is an operator, o2, at the top of the stack
              while (IsOperatorToken(opstoken.TokenValueType))
              {
                // pop o2 off the stack, onto the output queue;
                _output.Enqueue(_ops.Pop());
                if (_ops.Count > 0)
                  opstoken = (FormulaToken)_ops.Peek();
                else
                  break;
              }
            }
            // push o1 onto the operator stack.
            _ops.Push(token);
            break;

          case "*":
            token.TokenValueType = FormulaTokenType.Multiply;
            if (_ops.Count > 0)
            {
              opstoken = (FormulaToken)_ops.Peek();

              // while there is an operator, o2, at the top of the stack
              while (IsOperatorToken(opstoken.TokenValueType))
              {
                if (opstoken.TokenValueType == FormulaTokenType.Plus || opstoken.TokenValueType == FormulaTokenType.Minus || opstoken.TokenValueType == FormulaTokenType.Assignment)
                  break;
                else
                {
                  // Once we're in here, the following algorithm condition is satisfied.
                  // o1 is associative or left-associative and its precedence is less than (lower precedence) or equal to that of o2, or
                  // o1 is right-associative and its precedence is less than (lower precedence) that of o2,

                  // pop o2 off the stack, onto the output queue;
                  _output.Enqueue(_ops.Pop());
                  if (_ops.Count > 0)
                    opstoken = (FormulaToken)_ops.Peek();
                  else
                    break;
                }
              }
            }
            // push o1 onto the operator stack.
            _ops.Push(token);
            break;

          case "/":
            token.TokenValueType = FormulaTokenType.Divide;
            if (_ops.Count > 0)
            {
              opstoken = (FormulaToken)_ops.Peek();

              // while there is an operator, o2, at the top of the stack
              while (IsOperatorToken(opstoken.TokenValueType))
              {
                if (opstoken.TokenValueType == FormulaTokenType.Plus || opstoken.TokenValueType == FormulaTokenType.Minus || opstoken.TokenValueType == FormulaTokenType.Assignment)
                  break;
                else
                {
                  // Once we're in here, the following algorithm condition is satisfied.
                  // o1 is associative or left-associative and its precedence is less than (lower precedence) or equal to that of o2, or
                  // o1 is right-associative and its precedence is less than (lower precedence) that of o2,

                  // pop o2 off the stack, onto the output queue;
                  _output.Enqueue(_ops.Pop());
                  if (_ops.Count > 0)
                    opstoken = (FormulaToken)_ops.Peek();
                  else
                    break;
                }
              }
            }
            // push o1 onto the operator stack.
            _ops.Push(token);
            break;

          case "^":
            token.TokenValueType = FormulaTokenType.Exponent;
            // push o1 onto the operator stack.
            _ops.Push(token);
            break;

          case "~":
            token.TokenValueType = FormulaTokenType.UnaryMinus;
            // push o1 onto the operator stack.
            _ops.Push(token);
            break;

          case "(":
            token.TokenValueType = FormulaTokenType.LeftParenthesis;
            // If the token is a left parenthesis, then push it onto the stack.
            _ops.Push(token);
            break;

          case ")":
            token.TokenValueType = FormulaTokenType.RightParenthesis;
            if (_ops.Count > 0)
            {
              opstoken = (FormulaToken)_ops.Peek();
              // Until the token at the top of the stack is a left parenthesis
              while (opstoken.TokenValueType != FormulaTokenType.LeftParenthesis)
              {
                // pop operators off the stack onto the output queue
                _output.Enqueue(_ops.Pop());
                if (_ops.Count > 0)
                  opstoken = (FormulaToken)_ops.Peek();
                else
                {
                  // If the stack runs out without finding a left parenthesis,
                  // then there are mismatched parentheses.
                  throw new Exception("Unbalanced parenthesis!");
                }

              }
              // Pop the left parenthesis from the stack, but not onto the output queue.
              _ops.Pop();
            }

            if (_ops.Count > 0)
            {
              opstoken = (FormulaToken)_ops.Peek();
              // If the token at the top of the stack is a function token
              if (IsFunctionToken(opstoken.TokenValueType))
              {
                // pop it and onto the output queue.
                _output.Enqueue(_ops.Pop());
              }
            }
            break;

          case "pi":
            token.TokenValueType = FormulaTokenType.Constant;
            // If the token is a number, then add it to the output queue.
            _output.Enqueue(token);
            break;

          case "e":
            token.TokenValueType = FormulaTokenType.Constant;
            // If the token is a number, then add it to the output queue.
            _output.Enqueue(token);
            break;

          case "sin":
            token.TokenValueType = FormulaTokenType.Sine;
            // If the token is a function token, then push it onto the stack.
            _ops.Push(token);
            break;

          case "cos":
            token.TokenValueType = FormulaTokenType.Cosine;
            // If the token is a function token, then push it onto the stack.
            _ops.Push(token);
            break;

          case "tan":
            token.TokenValueType = FormulaTokenType.Tangent;
            // If the token is a function token, then push it onto the stack.
            _ops.Push(token);
            break;

          case "rnd":
            token.TokenValueType = FormulaTokenType.Round;
            // If the token is a function token, then push it onto the stack.
            _ops.Push(token);
            break;

          case "max":
            token.TokenValueType = FormulaTokenType.Max;
            // If the token is a function token, then push it onto the stack.
            _ops.Push(token);
            break;

          case "min":
            token.TokenValueType = FormulaTokenType.Min;
            // If the token is a function token, then push it onto the stack.
            _ops.Push(token);
            break;

          case ":":
            // If the token is an assignment token, then push it onto the stack.
            token.TokenValueType = FormulaTokenType.Assignment;
            // push o0 onto the operator stack.
            _ops.Push(token);
            break;

          case ",":
            if (_ops.Count > 0)
            {
              opstoken = (FormulaToken)_ops.Peek();
              // Until the token at the top of the stack is a left parenthesis
              while (opstoken.TokenValueType != FormulaTokenType.LeftParenthesis)
              {
                // pop operators off the stack onto the output queue
                _output.Enqueue(_ops.Pop());
                if (_ops.Count > 0)
                  opstoken = (FormulaToken)_ops.Peek();
                else
                {
                  // If the stack runs out without finding a left parenthesis,
                  // then there are mismatched parentheses.
                  throw new Exception("Couldn't find function start!");
                }
              }
            }
            break;

          default:
            if (Regex.IsMatch(parsedTokens[i], @"[a-z_]+"))
            {
              token.TokenValueType = FormulaTokenType.Variable;
              _output.Enqueue(token); // This is a variable.
            }
            break;
        }
      }

      // While there are still operator tokens in the stack:
      while (_ops.Count != 0)
      {
        opstoken = (FormulaToken)_ops.Pop();
        // If the operator token on the top of the stack is a parenthesis
        if (opstoken.TokenValueType == FormulaTokenType.LeftParenthesis)
        {
          // then there are mismatched parenthesis.
          throw new Exception("Unbalanced parenthesis!");
        }

        // Pop the operator onto the output queue.
        _output.Enqueue(opstoken);
      }

      PostfixExpression = string.Empty;
      foreach (object obj in _output)
      {
        opstoken = (FormulaToken)obj;
        PostfixExpression += $"{opstoken.TokenValue} ";
      }
    }

    private string GetTokenText(FormulaToken token)
    {
      string result = string.Empty;
      switch (token.TokenValueType)
      {
        case FormulaTokenType.Number:
          result = token.TokenValue;
          break;

        case FormulaTokenType.Variable:
          if (!_formulaVariables.TryGetValue(token.TokenValue, out result))
            result = "0.0";
          break;

        case FormulaTokenType.Constant:
          switch (token.TokenValue)
          {
            case "pi":
              result = Math.PI.ToString(CultureInfo.CurrentCulture);
              break;

            case "e":
              result = Math.E.ToString(CultureInfo.CurrentCulture);
              break;
          }
          break;
      }

      return result;
    }

    private double GetTokenValue(FormulaToken token)
    {
      return double.Parse(GetTokenText(token));
    }

    private bool IsFunctionToken(FormulaTokenType t)
    {
      bool result;
      switch (t)
      {
        case FormulaTokenType.Sine:
        case FormulaTokenType.Cosine:
        case FormulaTokenType.Tangent:
        case FormulaTokenType.Round:
        case FormulaTokenType.Max:
        case FormulaTokenType.Min:
          result = true;
          break;

        default:
          result = false;
          break;
      }

      return result;
    }

    private bool IsOperatorToken(FormulaTokenType t)
    {
      bool result;
      switch (t)
      {
        case FormulaTokenType.Plus:
        case FormulaTokenType.Minus:
        case FormulaTokenType.Multiply:
        case FormulaTokenType.Divide:
        case FormulaTokenType.Exponent:
        case FormulaTokenType.UnaryMinus:
          result = true;
          break;

        default:
          result = false;
          break;
      }

      return result;
    }
  }
}