using Godot;
using Godot.Collections;
using System;
using System.Linq;
using Object = Godot.Object;

public class inputmanager : Node
{
    /// <summary>
    /// This is a the core input manager script that handles the basic inputs
    /// of any device that allows controllers or V.Controller inputs for any
    /// fighting project.
    /// </summary>

    //Labels (Used for Demonstration Purposes)
    private Label Buffer_label;

    private Label Direction_label;

    //Floats
    private int Xscale = 1;
    private int Frames = 0;

    //Button Dictionaries
    private Dictionary<string, object> Button_History = new Dictionary<string, object>()
    {
        {"dir", Input_neutral},
        {"A", false},
        {"B", false},
        {"C", false},
        {"D", false},
        {"frame", 0}
    };

    private Dictionary<string, object> Buttons = new Dictionary<string, object>()
    {
        {"dir", Input_neutral},
        {"A", false},
        {"B", false},
        {"C", false},
        {"D", false}
    };

    private Dictionary<string, object> Prev_Buttons = new Dictionary<string, object>()
    {
        {"dir", Input_neutral},
        {"A", false},
        {"B", false},
        {"C", false},
        {"D", false}
    };

    //Command Buffer
    private Vector2[] Command_buffer;
    private const int Command_buffer_size = 6;
    private const int Command_buffer_timer_max = 10;
    private float Command_buffer_timer = Command_buffer_timer_max;
    
    //Vector Directions
    private static Vector2 Input_neutral = new Vector2(0, 0);
    private static Vector2 Input_up = new Vector2(0, 1);
    private static Vector2 Input_up_forward = new Vector2(1, 1);
    private static Vector2 Input_forward = new Vector2(1, 0);
    private static Vector2 Input_down_forward = new Vector2(1, -1);
    private static Vector2 Input_down = new Vector2(0, -1);
    private static Vector2 Input_down_back = new Vector2(-1, -1);
    private static Vector2 Input_back = new Vector2(-1, 0);
    private static Vector2 Input_up_back = new Vector2(-1, 1);

    //Button Lag
    private const int Button_lag = 0;
    private const int Button_buffer = 6;
    //Button Presses
    private bool Button_A;
    private bool Button_A_held;
    private bool Button_B;
    private bool Button_B_held;
    private bool Button_C;
    private bool Button_C_held;
    private bool Button_D;
    private bool Button_D_held;
    
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
        //Button_History.
        
        //Fills the command input array to its size.
        foreach (var _x in Enumerable.Range(0,Command_buffer_size))
        {
            Command_buffer.Append(Input_neutral);
        }

    }


  public override void _PhysicsProcess(float delta)
  {
      Frames += 1;
      /*All the functions required for this work. In addition its required
       / for these functions to be active under PhysicProcess for FrameRate consistancy;
       */
      GetInput();
      SetButtons();
      SetHeldButtons();
      SetCommandBuffer();
      GetChargeInput();
      Get_QCF();
      Get_QCB();
      Get_DP();
      Get_RDP();
      Get_HCF();
      Get_HCB();
      Get_DD();
      Get_FD();
      Get_BD();
      SetHistory();

      if (Input.IsActionJustPressed("swap"))
      {
          Xscale = -Xscale;
      }

      string directionText;
      if (Xscale == 1)
      {
          directionText = "RIGHT";
      }
      else
      {
          directionText = "LEFT";
      }
      Direction_label.Text = "Press Enter/Square(PS)/X(XB) to change direction facing. \n \n Current direction: " + directionText;
      //Used for Demonstration Purposes
      Buffer_label.Text = Command_buffer.ToString();

  }

  public void GetInput()
  {
      //Checks for opposite inputs, if there are, those inputs cancel each other.
      Vector2 button_dir = (Vector2) Buttons["dir"];
      bool bA = (bool) Buttons["Button_A"];
      bool bB = (bool) Buttons["Button_B"];
      bool bC = (bool) Buttons["Button_C"];
      bool bD = (bool) Buttons["Button_D"];
      
      //Joystick/Keyboard Movement
      if (Input.IsActionPressed("ui_up") && !Input.IsActionPressed("ui_down"))
      {
          button_dir.y = 1;
      }
      else if(Input.IsActionPressed("ui_down") && !Input.IsActionPressed("ui_down"))
      {
          button_dir.y = -1;
      }
      else
      {
          button_dir.y = 0;
      }
      
      if (Input.IsActionPressed("ui_right") && !Input.IsActionPressed("ui_left"))
      {
          button_dir.x = Xscale;
      }
      else if(Input.IsActionPressed("ui_left") && !Input.IsActionPressed("ui_right"))
      {
          button_dir.x = -Xscale;
      }
      else
      {
          button_dir.x = 0;
      }
      
      //Button Macro Inputs
      if (Input.IsActionPressed("A") || Input.IsActionPressed("macro"))
      {
          bA = true;
      }
      else
      {
          bA = false;
      }
      
      if (Input.IsActionPressed("B") || Input.IsActionPressed("macro"))
      {
          bB = true;
      }
      else
      {
          bB = false;
      }
      
      if (Input.IsActionPressed("C"))
      {
          bC= true;
      }
      else
      {
          bC= false;
      }
      
      if (Input.IsActionPressed("D"))
      {
          bD = true;
      }
      else
      {
          bD = false;
      }

  }

  public void SetHistory()
  {
      //Updates Dictionary array so that it can be saved for replays and can be checked during rollback.
      //Other functions also check previous values to determine inputs.


      if (Buttons.GetHashCode() != Prev_Buttons.GetHashCode())
      {
          Button_History.Append()
      }
  }

  public void SetButtons()
  {
      //Assign buttons booleans based on the collected inputs, in addition to setting the input buffer's timers.
  }

  public void SetHeldButtons()
  {
      //Assign and Sets Held Buttons, based on the current frame.
  }

  public void SetCommandBuffer()
  {
      //Sets the buffer that is used to determine command inputs by adding new value with each timeout/ new direction
      //Acts as a reference for motion inputs to be detected
  }

  public void GetChargeInput()
  {
      //Gets the joystick input and check if that input being held/charge
      //Think of G*ile from a certain fighting game with their Flash Kick
  }

  public void Get_QCF()
  {
      //Quarter-Circle Forward Input
  }

  public void Get_QCB()
  {
      //Quarter-Circle Back Input
      
  }

  public void Get_DP()
  {
      //Dragon Punch Input
  }

  public void Get_RDP()
  {
      //Reverse-Dragon Punch Input
  }

  public void Get_HCF()
  {
      
  }
  
  public void Get_HCB()
  {
      
  }
  
  public void Get_DD()
  {
      
  }
  
  public void Get_FD()
  {
      //Forward Dash
  }
  
  public void Get_BD()
  {
      //Back Dash
  }
}
