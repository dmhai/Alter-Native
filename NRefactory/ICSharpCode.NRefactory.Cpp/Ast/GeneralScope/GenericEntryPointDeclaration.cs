﻿using System.Collections.Generic;
using System.Linq;
using ICSharpCode.NRefactory.TypeSystem;
using ICSharpCode.NRefactory.Cpp.Ast;

namespace ICSharpCode.NRefactory.Cpp.Ast
{

    /// <summary>
    /// class Name&lt;TypeParameters&gt; : BaseTypes where Constraints;
    /// </summary>
    public class GenericEntryPointDeclaration : TypeDeclaration
    {
        public static readonly new GenericEntryPointDeclaration Null = new NullGenericEntryPointDeclaration();

        sealed class NullGenericEntryPointDeclaration : GenericEntryPointDeclaration
        {
            public override bool IsNull
            {
                get
                {
                    return true;
                }
            }

            public override S AcceptVisitor<T, S>(IAstVisitor<T, S> visitor, T data = default(T))
            {
                return default(S);
            }

            protected internal override bool DoMatch(AstNode other, PatternMatching.Match match)
            {
                return other == null || other.IsNull;
            }
        }

        public ClassType ClassType
        {
            get;
            set;
        }

        public string Name
        {
            get
            {
                return GetChildByRole(Roles.Identifier).Name;
            }
            set
            {
                SetChildByRole(Roles.Identifier, Identifier.Create(value, TextLocation.Empty));
            }
        }

        public Identifier NameToken
        {
            get
            {
                return GetChildByRole(Roles.Identifier);
            }
            set
            {
                SetChildByRole(Roles.Identifier, value);
            }
        }

        public AstNodeCollection<TypeParameterDeclaration> TypeParameters
        {
            get { return GetChildrenByRole(Roles.TypeParameter); }
        }

        public AstNodeCollection<AstType> BaseTypes
        {
            get { return GetChildrenByRole(BaseTypeRole); }
        }

        //public AstNodeCollection<Constraint> Constraints {
        //    get { return GetChildrenByRole (Roles.Constraint); }
        //}

        public CppTokenNode LBraceToken
        {
            get { return GetChildByRole(Roles.LBrace); }
        }

        public AstNodeCollection<AttributedNode> Members
        {
            get { return GetChildrenByRole(MemberRole); }
        }

        public CppTokenNode RBraceToken
        {
            get { return GetChildByRole(Roles.RBrace); }
        }

        public override S AcceptVisitor<T, S>(IAstVisitor<T, S> visitor, T data = default(T))
        {
            return visitor.VisitGenericEntryPointDeclaration(this, data);
        }

        protected internal override bool DoMatch(AstNode other, PatternMatching.Match match)
        {
            GenericEntryPointDeclaration o = other as GenericEntryPointDeclaration;
            return o != null && this.ClassType == o.ClassType && this.MatchAttributesAndModifiers(o, match)
                && MatchString(this.Name, o.Name) && this.TypeParameters.DoMatch(o.TypeParameters, match)
                && this.BaseTypes.DoMatch(o.BaseTypes, match) /*&& this.Constraints.DoMatch(o.Constraints, match)*/
                && this.Members.DoMatch(o.Members, match);
        }
    }
}
