using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour, ITimeTracker
{
    public static UIManager Instance { get; private set; }
    [Header("Status Bar")]
    //Tool equip slot on the status bar
    public Image toolEquipSlot;
    //Time UI
    public Text timeText;
    public Text dateText;


    [Header("Inventory System")]
    //The inventory panel
    public GameObject inventoryPanel;

    //The tool equip slot UI on the Inventory panel
    public HandInventorySlot toolHandSlot;

    //The tool slot UIs
    public InventorySlot[] toolSlots;

    //The item equip slot UI on the Inventory panel
    public HandInventorySlot itemHandSlot;

    //The item slot UIs
    public InventorySlot[] itemSlots;

    //Item info box
    public Text itemNameText;
    public Text itemDescriptionText;


    private void Awake()
    {
        //If there is more than one instance, destroy the extra
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            //Set the static instance to this instance
            Instance = this;
        }
    }

    private void Start()
    {
        RenderInventory();
        AssignSlotIndexes();

        //Add UIManager to the list of objects TimeManager will notify when the time updates
        TimeManager.Instance.RegisterTracker(this);
    }

    //Iterate through the slot UI elements and assign it its reference slot index
    public void AssignSlotIndexes()
    {
        for (int i = 0; i < toolSlots.Length; i++)
        {
            toolSlots[i].AssignIndex(i);
            itemSlots[i].AssignIndex(i);
        }
    }

    //Render the inventory screen to reflect the Player's Inventory. 
    public void RenderInventory()
    {
        //Get the inventory tool slots from Inventory Manager
        ItemData[] inventoryToolSlots = InventoryManager.Instance.tools;

        //Get the inventory item slots from Inventory Manager
        ItemData[] inventoryItemSlots = InventoryManager.Instance.items;

        //Render the Tool section
        RenderInventoryPanel(inventoryToolSlots, toolSlots);

        //Render the Item section
        RenderInventoryPanel(inventoryItemSlots, itemSlots);

        //Render the equipped slots
        toolHandSlot.Display(InventoryManager.Instance.equippedTool);
        itemHandSlot.Display(InventoryManager.Instance.equippedItem);

        //Get Tool Equip from InventoryManager
        ItemData equippedTool = InventoryManager.Instance.equippedTool;

        //Check if there is an item to display
        if (equippedTool != null)
        {
            //Switch the thumbnail over
            toolEquipSlot.sprite = equippedTool.thumbnail;

            toolEquipSlot.gameObject.SetActive(true);

            return;
        }

        toolEquipSlot.gameObject.SetActive(false);
    }

    //Iterate through a slot in a section and display them in the UI
    void RenderInventoryPanel(ItemData[] slots, InventorySlot[] uiSlots)
    {
        for (int i = 0; i < uiSlots.Length; i++)
        {
            //Display them accordingly
            uiSlots[i].Display(slots[i]);
        }
    }

    public void ToggleInventoryPanel()
    {
        //If the panel is hidden, show it and vice versa
        inventoryPanel.SetActive(!inventoryPanel.activeSelf);

        RenderInventory();
    }

    //Display Item info on the Item infobox
    public void DisplayItemInfo(ItemData data)
    {
        //If data is null, reset
        if (data == null)
        {
            itemNameText.text = "";
            itemDescriptionText.text = "";

            return;
        }

        itemNameText.text = data.name;
        itemDescriptionText.text = data.description;
    }

    //Callback to handle the UI for time
    public void ClockUpdate(GameTimestamp timestamp)
    {
        //Handle the time
        //Get the hours and minutes
        int hours = timestamp.hour;
        int minutes = timestamp.minute;

        //AM or PM
        string prefix = "AM ";

        //Convert hours to 12 hour clock
        if (hours > 12)
        {
            //Time becomes PM 
            prefix = "PM ";
            hours = hours - 12;
            Debug.Log(hours);
        }

        //Format it for the time text display
        timeText.text = prefix + hours + ":" + minutes.ToString("00");

        //Handle the Date
        int day = timestamp.day;
        string season = timestamp.season.ToString();
        string dayOfTheWeek = timestamp.GetDayOfTheWeek().ToString();

        //Format it for the date text display
        dateText.text = season + " " + day + " (" + dayOfTheWeek + ")";

    }
}