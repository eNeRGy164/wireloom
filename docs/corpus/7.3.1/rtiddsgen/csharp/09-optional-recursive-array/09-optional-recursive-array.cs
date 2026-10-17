
/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 09-optional-recursive-array.idl
using RTI Code Generator (rtiddsgen) version 4.3.1.
The rtiddsgen tool is part of the RTI Connext DDS distribution.
For more information, type 'rtiddsgen -help' at a command shell
or consult the Code Generator User's Manual.
*/

using System;
using System.Reflection;
using System.Collections.Generic;
using Rti.Types;
using System.Linq;
using Omg.Types;

namespace CorpusOptionalRecursiveArray
{

    public class Node :  IEquatable<Node>
    {
        public int value { get; set; }
        [Optional]
        public global::CorpusOptionalRecursiveArray.Node[] children { get; set; }

        public Node()
        {
        }

        public Node(int  value, global::CorpusOptionalRecursiveArray.Node [] children)
        {
            this.value = value;
            this.children = children;
        }

        public Node(Node other)
        {
            if (other == null)
            {
                return;
            }

            this.value = other.value;
            if(other.children != null)
            {
                this.children = new global::CorpusOptionalRecursiveArray.Node[2];
                for( int i1 = 0; i1 < 2; i1++)
                {
                    children[i1] = new global::CorpusOptionalRecursiveArray.Node(other.children[i1]);
                }
            }

        }

        public override int GetHashCode()
        {
            var hash = new HashCode();

            hash.Add(this.value);
            if (this.children != null)
            {
                hash.Add(this.children[0]);
            }
            else
            {
                hash.Add(-1);
            }

            return hash.ToHashCode();
        }

        public bool Equals(Node other)
        {
            if (other == null)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return this.value.Equals(other.value) && 
            (ReferenceEquals(this.children, other.children) || (this.children != null && other.children != null && this.children.SequenceEqual(other.children)));
        }

        public override bool Equals(object obj) => this.Equals(obj as Node);

        public override string ToString() => NodeSupport.Instance.ToString(this);
    }

} // namespace CorpusOptionalRecursiveArray
