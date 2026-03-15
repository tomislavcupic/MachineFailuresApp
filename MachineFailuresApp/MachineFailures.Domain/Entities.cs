namespace MachineFailures.Domain;

public class Machine
{
    public int Id {get ; set; }
    public string Name {get ; set; }
    public int CategoryId {get ; set; }

};

public class Failure
{
    public int Id {get ; set; }
    public int MachineId {get ; set; }
    public string Name {get ; set; }
    public string Description {get ; set; }    
    public DateTime StartOfFailure {get ; set; }
    public DateTime? EndOfFailure {get ; set; }

}

public class Category
{
    public int Id {get ; set; }
    public string Name {get ; set; }
    public string? Description {get ; set; }
}