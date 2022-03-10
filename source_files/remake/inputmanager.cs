using Godot;
using System;

public class inputmanager : Node
{
    /// <summary>
    /// This is a the core input manager script that handles the basic inputs
    /// of any device that allows controllers or V.Controller inputs for any
    /// fighting project.
    /// </summary>
    
    //Labels
    private Label Buffer_label;
    private Label Direction_label;
    
    //Floats
    private int Xscale = 1;
    private int Frames = 0;
    
    //Button Dictionaries
    private Directory Button_History = new Directory();
    private Directory Buttons;
    private Directory Prev_Buttons;
    
    //Command Buffer
    
    
    //Vector Directions
    private Vector2 Input_neutral = new Vector2(0, 0);
    private Vector2 Input_up = new Vector2(0, 1);
    private Vector2 Input_up_forward = new Vector2(1, 1);
    private Vector2 Input_forward = new Vector2(1, 0);
    private Vector2 Input_down_forward = new Vector2(1, -1);
    private Vector2 Input_down = new Vector2(0, -1);
    private Vector2 Input_down_back = new Vector2(-1, -1);
    private Vector2 Input_back = new Vector2(-1, 0);
    private Vector2 Input_up_back = new Vector2(-1, 1);



    //Button Lag
    private const int Button_lag = 0;
    private const int Button_buffer = 6;
    //Button Presses
    
    //Button Timer
    private int ButtonA_timer = Button_lag + Button_buffer;
    private int ButtonB_timer = Button_lag + Button_buffer;
    private int ButtonC_timer = Button_lag + Button_buffer;
    private int ButtonD_timer = Button_lag + Button_buffer;
    
    //Motion Speed
    private const int Full_charge = 30;
    private const int Max_charge = 45;

    private int bcharge_value = 0;

    private int dcharge_value = 0;
    //Command Input Checks
    private bool Qcf = false;
    private bool Qcb = false;
    private bool Dp = false;
    private bool Rdp = false;
    private bool Hcf = false;
    private bool Hcb = false;
    private bool Dd = false;
    private bool Dir = false;
    private bool Fdash = false;
    private bool Bdash = false;
    private bool Dcharge = false;
    private bool Bcharge = false;
    
    public override void _Ready()
    {
        Buffer_label = GetNode<Label>("BufferLabel");
        Direction_label = GetNode<Label>("DirectionLabel");

    }


//  public override void _Process(float delta)
//  {
//      
//  }
}
