using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BookSpecificScene2 : MonoBehaviour
{


    [NonSerialized] public Sprite bookCover;
    public string bookName;
    public string bookAuthor; 
    public string bookDescription;
    public float bookProgress;
    
    
    //
    private GameObject canvasBookTextContainer; 
    private TextMeshProUGUI canvasBookTitleTMP;
    private TextMeshProUGUI canvasBookAuthorTMP;
    private TextMeshProUGUI canvasBookDescriptionTMP;
    private TextMeshProUGUI canvasAmtCompleteTMP;
    
    private BookManagerScene2 bookManager;
    
    
    public bool selected;
    [NonSerialized] public TransformDot bookdot;

    private MeshRenderer bookRenderer;
    
    public Material inactiveMaterial;
    public Material activeMaterial;
    
    //lay the pop up above the book

    //canvas and elements
    private Canvas smallCanvas;
    
    private float bookCardOffset = 0.2f;
    private Image smallBackground;
    private Image smallMask;
    [NonSerialized] public Image bookCoverImage;

    private WallBannerCanvasScene2 wallBanner; 
    
    private void Awake()
    {

        
        smallCanvas = GameObject.Find("BookInformationCanvas").GetComponent<Canvas>();
        bookCoverImage = GameObject.Find("BookCover").GetComponent<Image>();
        bookManager = GetComponentInParent<BookManagerScene2>();
        bookManager.onBookChanged += OnBookChanged;
        bookRenderer = gameObject.GetComponent<MeshRenderer>();
        wallBanner = GameObject.FindObjectOfType<WallBannerCanvasScene2>();
        string originalString = "bookdot.name";
        string capitalizedString = char.ToUpper(originalString[0]) + originalString.Substring(1);

        if (bookdot != null)
        {
            bookdot.name = capitalizedString;
        }
        
        
        canvasBookTextContainer = GameObject.Find("TextContainer1");
        canvasBookTitleTMP = canvasBookTextContainer.GetComponentsInChildren<TextMeshProUGUI>()[0];
        canvasBookAuthorTMP = canvasBookTextContainer.GetComponentsInChildren<TextMeshProUGUI>()[1];
        canvasBookDescriptionTMP = canvasBookTextContainer.GetComponentsInChildren<TextMeshProUGUI>()[2];

    }

    private void OnBookChanged(TransformDot dot)
    {
        if (dot == bookdot)
        {
            selected = true;
            bookRenderer.material = activeMaterial;
            
            smallCanvas.transform.position = new Vector3(transform.position.x, 
                transform.position.y + bookCardOffset,
                transform.position.z);

            bookCoverImage.sprite = bookCover;

            canvasBookTitleTMP.text = bookName;
            canvasBookAuthorTMP.text = bookAuthor;
            canvasBookDescriptionTMP.text = bookDescription;

            wallBanner.title.text = bookName;
            wallBanner.author.text = bookAuthor;
            wallBanner.bookImage.sprite = bookCover;
        }
        else
        {
            selected = false;
            bookRenderer.material = inactiveMaterial;
        }
    }
 
}
