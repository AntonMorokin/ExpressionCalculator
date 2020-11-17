using Calculation.Model;
using Calculation.Model.Functions.Binary;
using Calculation.Model.Functions.Unary;
using Parsing.Model;
using System;

namespace Parsing
{
    public class TreeConverter
    {
        public IHasValue Convert(TreeNode treeNode)
        {
            var value = treeNode.Value;

            if (value is Number)
            {
                return value;
            }

            if (value is UnaryFunction uf)
            {
                var unaryFunctionValue = Convert(treeNode.LeftChild);
                
                uf.SetArguments(unaryFunctionValue);
                
                return uf;
            }

            if (value is BinaryFunction bf)
            {
                var firstValue = Convert(treeNode.LeftChild);
                var secondValue = Convert(treeNode.RightChild);

                bf.SetArguments(firstValue, secondValue);

                return bf;
            }

            throw new InvalidOperationException($"Unknown value type {value?.GetType().Name}");
        }
    }
}
