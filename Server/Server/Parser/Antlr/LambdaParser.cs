//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     ANTLR Version: 4.10.1
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// Generated from /run/media/rd211/SHARED/find_my_lambda/Server/Parser/Antlr/Lambda.g4 by ANTLR 4.10.1

// Unreachable code detected
#pragma warning disable 0162
// The variable '...' is assigned but its value is never used
#pragma warning disable 0219
// Missing XML comment for publicly visible type or member '...'
#pragma warning disable 1591
// Ambiguous reference in cref attribute
#pragma warning disable 419

using System;
using System.IO;
using System.Text;
using System.Diagnostics;
using System.Collections.Generic;
using Antlr4.Runtime;
using Antlr4.Runtime.Atn;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using DFA = Antlr4.Runtime.Dfa.DFA;

[System.CodeDom.Compiler.GeneratedCode("ANTLR", "4.10.1")]
[System.CLSCompliant(false)]
public partial class LambdaParser : Parser {
	protected static DFA[] decisionToDFA;
	protected static PredictionContextCache sharedContextCache = new PredictionContextCache();
	public const int
		T__0=1, T__1=2, T__2=3, T__3=4, T__4=5, T__5=6, T__6=7, T__7=8, STRING=9, 
		CHAR=10, INTEGER=11, REAL=12, WS=13;
	public const int
		RULE_input = 0, RULE_tuple = 1, RULE_arr = 2, RULE_value = 3;
	public static readonly string[] ruleNames = {
		"input", "tuple", "arr", "value"
	};

	private static readonly string[] _LiteralNames = {
		null, "'('", "','", "')'", "'['", "']'", "'true'", "'false'", "'null'"
	};
	private static readonly string[] _SymbolicNames = {
		null, null, null, null, null, null, null, null, null, "STRING", "CHAR", 
		"INTEGER", "REAL", "WS"
	};
	public static readonly IVocabulary DefaultVocabulary = new Vocabulary(_LiteralNames, _SymbolicNames);

	[NotNull]
	public override IVocabulary Vocabulary
	{
		get
		{
			return DefaultVocabulary;
		}
	}

	public override string GrammarFileName { get { return "Lambda.g4"; } }

	public override string[] RuleNames { get { return ruleNames; } }

	public override int[] SerializedAtn { get { return _serializedATN; } }

	static LambdaParser() {
		decisionToDFA = new DFA[_ATN.NumberOfDecisions];
		for (int i = 0; i < _ATN.NumberOfDecisions; i++) {
			decisionToDFA[i] = new DFA(_ATN.GetDecisionState(i), i);
		}
	}

		public LambdaParser(ITokenStream input) : this(input, Console.Out, Console.Error) { }

		public LambdaParser(ITokenStream input, TextWriter output, TextWriter errorOutput)
		: base(input, output, errorOutput)
	{
		Interpreter = new ParserATNSimulator(this, _ATN, decisionToDFA, sharedContextCache);
	}

	public partial class InputContext : ParserRuleContext {
		[System.Diagnostics.DebuggerNonUserCode] public ValueContext value() {
			return GetRuleContext<ValueContext>(0);
		}
		[System.Diagnostics.DebuggerNonUserCode] public ITerminalNode Eof() { return GetToken(LambdaParser.Eof, 0); }
		public InputContext(ParserRuleContext parent, int invokingState)
			: base(parent, invokingState)
		{
		}
		public override int RuleIndex { get { return RULE_input; } }
		[System.Diagnostics.DebuggerNonUserCode]
		public override void EnterRule(IParseTreeListener listener) {
			ILambdaListener typedListener = listener as ILambdaListener;
			if (typedListener != null) typedListener.EnterInput(this);
		}
		[System.Diagnostics.DebuggerNonUserCode]
		public override void ExitRule(IParseTreeListener listener) {
			ILambdaListener typedListener = listener as ILambdaListener;
			if (typedListener != null) typedListener.ExitInput(this);
		}
		[System.Diagnostics.DebuggerNonUserCode]
		public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor) {
			ILambdaVisitor<TResult> typedVisitor = visitor as ILambdaVisitor<TResult>;
			if (typedVisitor != null) return typedVisitor.VisitInput(this);
			else return visitor.VisitChildren(this);
		}
	}

	[RuleVersion(0)]
	public InputContext input() {
		InputContext _localctx = new InputContext(Context, State);
		EnterRule(_localctx, 0, RULE_input);
		try {
			EnterOuterAlt(_localctx, 1);
			{
			State = 8;
			value();
			State = 9;
			Match(Eof);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			ErrorHandler.ReportError(this, re);
			ErrorHandler.Recover(this, re);
		}
		finally {
			ExitRule();
		}
		return _localctx;
	}

	public partial class TupleContext : ParserRuleContext {
		[System.Diagnostics.DebuggerNonUserCode] public ValueContext[] value() {
			return GetRuleContexts<ValueContext>();
		}
		[System.Diagnostics.DebuggerNonUserCode] public ValueContext value(int i) {
			return GetRuleContext<ValueContext>(i);
		}
		public TupleContext(ParserRuleContext parent, int invokingState)
			: base(parent, invokingState)
		{
		}
		public override int RuleIndex { get { return RULE_tuple; } }
		[System.Diagnostics.DebuggerNonUserCode]
		public override void EnterRule(IParseTreeListener listener) {
			ILambdaListener typedListener = listener as ILambdaListener;
			if (typedListener != null) typedListener.EnterTuple(this);
		}
		[System.Diagnostics.DebuggerNonUserCode]
		public override void ExitRule(IParseTreeListener listener) {
			ILambdaListener typedListener = listener as ILambdaListener;
			if (typedListener != null) typedListener.ExitTuple(this);
		}
		[System.Diagnostics.DebuggerNonUserCode]
		public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor) {
			ILambdaVisitor<TResult> typedVisitor = visitor as ILambdaVisitor<TResult>;
			if (typedVisitor != null) return typedVisitor.VisitTuple(this);
			else return visitor.VisitChildren(this);
		}
	}

	[RuleVersion(0)]
	public TupleContext tuple() {
		TupleContext _localctx = new TupleContext(Context, State);
		EnterRule(_localctx, 2, RULE_tuple);
		int _la;
		try {
			EnterOuterAlt(_localctx, 1);
			{
			State = 11;
			Match(T__0);
			State = 12;
			value();
			State = 17;
			ErrorHandler.Sync(this);
			_la = TokenStream.LA(1);
			while (_la==T__1) {
				{
				{
				State = 13;
				Match(T__1);
				State = 14;
				value();
				}
				}
				State = 19;
				ErrorHandler.Sync(this);
				_la = TokenStream.LA(1);
			}
			State = 20;
			Match(T__2);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			ErrorHandler.ReportError(this, re);
			ErrorHandler.Recover(this, re);
		}
		finally {
			ExitRule();
		}
		return _localctx;
	}

	public partial class ArrContext : ParserRuleContext {
		[System.Diagnostics.DebuggerNonUserCode] public ValueContext[] value() {
			return GetRuleContexts<ValueContext>();
		}
		[System.Diagnostics.DebuggerNonUserCode] public ValueContext value(int i) {
			return GetRuleContext<ValueContext>(i);
		}
		public ArrContext(ParserRuleContext parent, int invokingState)
			: base(parent, invokingState)
		{
		}
		public override int RuleIndex { get { return RULE_arr; } }
		[System.Diagnostics.DebuggerNonUserCode]
		public override void EnterRule(IParseTreeListener listener) {
			ILambdaListener typedListener = listener as ILambdaListener;
			if (typedListener != null) typedListener.EnterArr(this);
		}
		[System.Diagnostics.DebuggerNonUserCode]
		public override void ExitRule(IParseTreeListener listener) {
			ILambdaListener typedListener = listener as ILambdaListener;
			if (typedListener != null) typedListener.ExitArr(this);
		}
		[System.Diagnostics.DebuggerNonUserCode]
		public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor) {
			ILambdaVisitor<TResult> typedVisitor = visitor as ILambdaVisitor<TResult>;
			if (typedVisitor != null) return typedVisitor.VisitArr(this);
			else return visitor.VisitChildren(this);
		}
	}

	[RuleVersion(0)]
	public ArrContext arr() {
		ArrContext _localctx = new ArrContext(Context, State);
		EnterRule(_localctx, 4, RULE_arr);
		int _la;
		try {
			State = 35;
			ErrorHandler.Sync(this);
			switch ( Interpreter.AdaptivePredict(TokenStream,2,Context) ) {
			case 1:
				EnterOuterAlt(_localctx, 1);
				{
				State = 22;
				Match(T__3);
				State = 23;
				value();
				State = 28;
				ErrorHandler.Sync(this);
				_la = TokenStream.LA(1);
				while (_la==T__1) {
					{
					{
					State = 24;
					Match(T__1);
					State = 25;
					value();
					}
					}
					State = 30;
					ErrorHandler.Sync(this);
					_la = TokenStream.LA(1);
				}
				State = 31;
				Match(T__4);
				}
				break;
			case 2:
				EnterOuterAlt(_localctx, 2);
				{
				State = 33;
				Match(T__3);
				State = 34;
				Match(T__4);
				}
				break;
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			ErrorHandler.ReportError(this, re);
			ErrorHandler.Recover(this, re);
		}
		finally {
			ExitRule();
		}
		return _localctx;
	}

	public partial class ValueContext : ParserRuleContext {
		[System.Diagnostics.DebuggerNonUserCode] public ITerminalNode STRING() { return GetToken(LambdaParser.STRING, 0); }
		[System.Diagnostics.DebuggerNonUserCode] public ITerminalNode CHAR() { return GetToken(LambdaParser.CHAR, 0); }
		[System.Diagnostics.DebuggerNonUserCode] public ITerminalNode REAL() { return GetToken(LambdaParser.REAL, 0); }
		[System.Diagnostics.DebuggerNonUserCode] public ITerminalNode INTEGER() { return GetToken(LambdaParser.INTEGER, 0); }
		[System.Diagnostics.DebuggerNonUserCode] public TupleContext tuple() {
			return GetRuleContext<TupleContext>(0);
		}
		[System.Diagnostics.DebuggerNonUserCode] public ArrContext arr() {
			return GetRuleContext<ArrContext>(0);
		}
		public ValueContext(ParserRuleContext parent, int invokingState)
			: base(parent, invokingState)
		{
		}
		public override int RuleIndex { get { return RULE_value; } }
		[System.Diagnostics.DebuggerNonUserCode]
		public override void EnterRule(IParseTreeListener listener) {
			ILambdaListener typedListener = listener as ILambdaListener;
			if (typedListener != null) typedListener.EnterValue(this);
		}
		[System.Diagnostics.DebuggerNonUserCode]
		public override void ExitRule(IParseTreeListener listener) {
			ILambdaListener typedListener = listener as ILambdaListener;
			if (typedListener != null) typedListener.ExitValue(this);
		}
		[System.Diagnostics.DebuggerNonUserCode]
		public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor) {
			ILambdaVisitor<TResult> typedVisitor = visitor as ILambdaVisitor<TResult>;
			if (typedVisitor != null) return typedVisitor.VisitValue(this);
			else return visitor.VisitChildren(this);
		}
	}

	[RuleVersion(0)]
	public ValueContext value() {
		ValueContext _localctx = new ValueContext(Context, State);
		EnterRule(_localctx, 6, RULE_value);
		try {
			State = 46;
			ErrorHandler.Sync(this);
			switch (TokenStream.LA(1)) {
			case STRING:
				EnterOuterAlt(_localctx, 1);
				{
				State = 37;
				Match(STRING);
				}
				break;
			case CHAR:
				EnterOuterAlt(_localctx, 2);
				{
				State = 38;
				Match(CHAR);
				}
				break;
			case REAL:
				EnterOuterAlt(_localctx, 3);
				{
				State = 39;
				Match(REAL);
				}
				break;
			case INTEGER:
				EnterOuterAlt(_localctx, 4);
				{
				State = 40;
				Match(INTEGER);
				}
				break;
			case T__0:
				EnterOuterAlt(_localctx, 5);
				{
				State = 41;
				tuple();
				}
				break;
			case T__3:
				EnterOuterAlt(_localctx, 6);
				{
				State = 42;
				arr();
				}
				break;
			case T__5:
				EnterOuterAlt(_localctx, 7);
				{
				State = 43;
				Match(T__5);
				}
				break;
			case T__6:
				EnterOuterAlt(_localctx, 8);
				{
				State = 44;
				Match(T__6);
				}
				break;
			case T__7:
				EnterOuterAlt(_localctx, 9);
				{
				State = 45;
				Match(T__7);
				}
				break;
			default:
				throw new NoViableAltException(this);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			ErrorHandler.ReportError(this, re);
			ErrorHandler.Recover(this, re);
		}
		finally {
			ExitRule();
		}
		return _localctx;
	}

	private static int[] _serializedATN = {
		4,1,13,49,2,0,7,0,2,1,7,1,2,2,7,2,2,3,7,3,1,0,1,0,1,0,1,1,1,1,1,1,1,1,
		5,1,16,8,1,10,1,12,1,19,9,1,1,1,1,1,1,2,1,2,1,2,1,2,5,2,27,8,2,10,2,12,
		2,30,9,2,1,2,1,2,1,2,1,2,3,2,36,8,2,1,3,1,3,1,3,1,3,1,3,1,3,1,3,1,3,1,
		3,3,3,47,8,3,1,3,0,0,4,0,2,4,6,0,0,55,0,8,1,0,0,0,2,11,1,0,0,0,4,35,1,
		0,0,0,6,46,1,0,0,0,8,9,3,6,3,0,9,10,5,0,0,1,10,1,1,0,0,0,11,12,5,1,0,0,
		12,17,3,6,3,0,13,14,5,2,0,0,14,16,3,6,3,0,15,13,1,0,0,0,16,19,1,0,0,0,
		17,15,1,0,0,0,17,18,1,0,0,0,18,20,1,0,0,0,19,17,1,0,0,0,20,21,5,3,0,0,
		21,3,1,0,0,0,22,23,5,4,0,0,23,28,3,6,3,0,24,25,5,2,0,0,25,27,3,6,3,0,26,
		24,1,0,0,0,27,30,1,0,0,0,28,26,1,0,0,0,28,29,1,0,0,0,29,31,1,0,0,0,30,
		28,1,0,0,0,31,32,5,5,0,0,32,36,1,0,0,0,33,34,5,4,0,0,34,36,5,5,0,0,35,
		22,1,0,0,0,35,33,1,0,0,0,36,5,1,0,0,0,37,47,5,9,0,0,38,47,5,10,0,0,39,
		47,5,12,0,0,40,47,5,11,0,0,41,47,3,2,1,0,42,47,3,4,2,0,43,47,5,6,0,0,44,
		47,5,7,0,0,45,47,5,8,0,0,46,37,1,0,0,0,46,38,1,0,0,0,46,39,1,0,0,0,46,
		40,1,0,0,0,46,41,1,0,0,0,46,42,1,0,0,0,46,43,1,0,0,0,46,44,1,0,0,0,46,
		45,1,0,0,0,47,7,1,0,0,0,4,17,28,35,46
	};

	public static readonly ATN _ATN =
		new ATNDeserializer().Deserialize(_serializedATN);


}
