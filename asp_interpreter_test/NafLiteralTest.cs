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
                IsBinaryOperation: false,
                IsClassicalLiteral: false,
                BinaryOperation: null,
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
                IsBinaryOperation: false,
                IsClassicalLiteral: true,
                BinaryOperation: null} && nafLiteral.ClassicalLiteral == literal);
    }
    
    [Test]
    public void CreatesNafLiteralWithBinaryOperation()
    {
        //It does not make a difference what the terms are for this test
        var binaryOperation = new BinaryOperation(
            new AnonymusVariableTerm(),
            new Equality(),
            new AnonymusVariableTerm());
        
        var nafLiteral = new NafLiteral(binaryOperation);
        
        Assert.That(nafLiteral is 
            {
                IsNafNegated:false,
                IsBinaryOperation: true,
                IsClassicalLiteral: false,
                ClassicalLiteral: null} && nafLiteral.BinaryOperation == binaryOperation);
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
                IsBinaryOperation: false,
                IsClassicalLiteral: true,
                BinaryOperation: null} && nafLiteral.ClassicalLiteral == literal);
    }
    
    [Test]
    public void AddsBinaryOperationToNafLiteral()
    {
        var nafLiteral = new NafLiteral();
        var binaryOperation = new BinaryOperation(
            new AnonymusVariableTerm(),
            new Equality(),
            new AnonymusVariableTerm());
        
        nafLiteral.AddBinaryOperation(binaryOperation);
        
        Assert.That(nafLiteral is 
            {
                IsNafNegated:false,
                IsBinaryOperation: true,
                IsClassicalLiteral: false,
                ClassicalLiteral: null} && nafLiteral.BinaryOperation == binaryOperation);
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
    public void ThrowsOnAddingBinaryOperationTwice()
    {
        var nafLiteral = new NafLiteral();
        var binaryOperation = new BinaryOperation(
            new AnonymusVariableTerm(),
            new Equality(),
            new AnonymusVariableTerm());
        
        nafLiteral.AddBinaryOperation(binaryOperation);
        
        Assert.Throws<InvalidOperationException>(() => nafLiteral.AddBinaryOperation(binaryOperation));
    }
    
    [Test]
    public void ThrowsOnAddingNullClassicalLiteral()
    {
        var nafLiteral = new NafLiteral();
        
        Assert.Throws<ArgumentNullException>(() => nafLiteral.AddClassicalLiteral(null, true));
    }
    
    [Test]
    public void ThrowsOnAddingNullBinaryOperation()
    {
        var nafLiteral = new NafLiteral();
        
        Assert.Throws<ArgumentNullException>(() => nafLiteral.AddBinaryOperation(null));
    }
    
    [Test]
    public void ThrowsOnAddingClassicalLiteralToBinaryOperation()
    {
        var literal = new ClassicalLiteral("a", false, []);
        var binaryOperation = new BinaryOperation(
            new AnonymusVariableTerm(),
            new Equality(),
            new AnonymusVariableTerm());

        var nafLiteral = new NafLiteral(binaryOperation);
        Assert.Throws<InvalidOperationException>(() => nafLiteral.AddClassicalLiteral(literal, true));
    }
    
    [Test]
    public void ThrowsOnAddingBinaryOperationToClassicalLiteral()
    {
        var literal = new ClassicalLiteral("a", false, []);
        var binaryOperation = new BinaryOperation(
            new AnonymusVariableTerm(),
            new Equality(),
            new AnonymusVariableTerm());
        
        var nafLiteral = new NafLiteral(literal, true);
        Assert.Throws<InvalidOperationException>(() => nafLiteral.AddBinaryOperation(binaryOperation));
    }
}