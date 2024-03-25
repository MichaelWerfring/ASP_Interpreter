using asp_interpreter_lib.Types;
using asp_interpreter_lib.Types.BinaryOperations;
using asp_interpreter_lib.Types.Terms;

namespace asp_interpreter_test;

public class NafLiteralTest
{
    [Test]
    public void CreatesEmptyNafLiteralAsDefault()
    {
        var nafLiteral = new NafLiteral();
        
        Assert.That(nafLiteral is 
            {
                IsNafNegated:false,
                IsBuiltinAtom: false,
                IsClassicalLiteral: false,
                BuiltinAtom: null,
                ClassicalLiteral:null});
    }
    
    [Test]
    public void CreatesNafLiteralWithClassicalLiteral()
    {
        var literal = new ClassicalLiteral("a", false, []);
        var nafLiteral = new NafLiteral(literal, true);
        
        Assert.That(nafLiteral is 
            {
                IsNafNegated:true,
                IsBuiltinAtom: false,
                IsClassicalLiteral: true,
                BuiltinAtom: null} && nafLiteral.ClassicalLiteral == literal);
    }
    
    [Test]
    public void CreatesNafLiteralWithBuiltinAtom()
    {
        //It does not make a difference what the terms are for this test
        var builtinAtom = new BuiltinAtom(
            new AnonymusVariableTerm(),
            new Equality(new AnonymusVariableTerm(), new AnonymusVariableTerm()),
            new AnonymusVariableTerm());
        
        var nafLiteral = new NafLiteral(builtinAtom);
        
        Assert.That(nafLiteral is 
            {
                IsNafNegated:false,
                IsBuiltinAtom: true,
                IsClassicalLiteral: false,
                ClassicalLiteral: null} && nafLiteral.BuiltinAtom == builtinAtom);
    }
    
    [Test]
    public void AddsClassicalLiteralToNafLiteral()
    {
        var nafLiteral = new NafLiteral();
        var literal = new ClassicalLiteral("a", false, []);
        
        nafLiteral.AddClassicalLiteral(literal, true);
        
        Assert.That(nafLiteral is 
            {
                IsNafNegated:true,
                IsBuiltinAtom: false,
                IsClassicalLiteral: true,
                BuiltinAtom: null} && nafLiteral.ClassicalLiteral == literal);
    }
    
    [Test]
    public void AddsBuiltinAtomToNafLiteral()
    {
        var nafLiteral = new NafLiteral();
        var builtinAtom = new BuiltinAtom(
            new AnonymusVariableTerm(),
            new Equality(new AnonymusVariableTerm(), new AnonymusVariableTerm()),
            new AnonymusVariableTerm());
        
        nafLiteral.AddBuiltinAtom(builtinAtom);
        
        Assert.That(nafLiteral is 
            {
                IsNafNegated:false,
                IsBuiltinAtom: true,
                IsClassicalLiteral: false,
                ClassicalLiteral: null} && nafLiteral.BuiltinAtom == builtinAtom);
    }
    
    [Test]
    public void ThrowsOnAddingClassicalLiteralTwice()
    {
        var nafLiteral = new NafLiteral();
        var literal = new ClassicalLiteral("a", false, []);
        
        nafLiteral.AddClassicalLiteral(literal, true);
        
        Assert.Throws<InvalidOperationException>(() => nafLiteral.AddClassicalLiteral(literal, true));
    }
    
    [Test]
    public void ThrowsOnAddingBuiltinAtomTwice()
    {
        var nafLiteral = new NafLiteral();
        var builtinAtom = new BuiltinAtom(
            new AnonymusVariableTerm(),
            new Equality(new AnonymusVariableTerm(), new AnonymusVariableTerm()),
            new AnonymusVariableTerm());
        
        nafLiteral.AddBuiltinAtom(builtinAtom);
        
        Assert.Throws<InvalidOperationException>(() => nafLiteral.AddBuiltinAtom(builtinAtom));
    }
    
    [Test]
    public void ThrowsOnAddingNullClassicalLiteral()
    {
        var nafLiteral = new NafLiteral();
        
        Assert.Throws<ArgumentNullException>(() => nafLiteral.AddClassicalLiteral(null, true));
    }
    
    [Test]
    public void ThrowsOnAddingNullBuiltinAtom()
    {
        var nafLiteral = new NafLiteral();
        
        Assert.Throws<ArgumentNullException>(() => nafLiteral.AddBuiltinAtom(null));
    }
    
    [Test]
    public void ThrowsOnAddingClassicalLiteralToBuiltinAtom()
    {
        var literal = new ClassicalLiteral("a", false, []);
        var builtinAtom = new BuiltinAtom(
            new AnonymusVariableTerm(),
            new Equality(new AnonymusVariableTerm(), new AnonymusVariableTerm()),
            new AnonymusVariableTerm());

        var nafLiteral = new NafLiteral(builtinAtom);
        Assert.Throws<InvalidOperationException>(() => nafLiteral.AddClassicalLiteral(literal, true));
    }
    
    [Test]
    public void ThrowsOnAddingBuiltinAtomToClassicalLiteral()
    {
        var literal = new ClassicalLiteral("a", false, []);
        var builtinAtom = new BuiltinAtom(
            new AnonymusVariableTerm(),
            new Equality(new AnonymusVariableTerm(), new AnonymusVariableTerm()),
            new AnonymusVariableTerm());
        
        var nafLiteral = new NafLiteral(literal, true);
        Assert.Throws<InvalidOperationException>(() => nafLiteral.AddBuiltinAtom(builtinAtom));
    }
}