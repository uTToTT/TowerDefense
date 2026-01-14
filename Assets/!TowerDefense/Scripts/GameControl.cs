using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameControl : Loader<GameControl>
{
    [SerializeField] private int _startMoney;
    [SerializeField] private float _speedMultiply;
    [SerializeField] private TowerStage _stage;
    [Header("Locks")]
    [SerializeField] private bool _firstTowerFirstSpecLock;
    [SerializeField] private bool _firstTowerSecondSpecLock;
    [Space]
    [SerializeField] private bool _secondTowerFirstSpecLock;
    [SerializeField] private bool _secondTowerSecondSpecLock;
    [Space]
    [SerializeField] private bool _thirdTowerFirstSpecLock;
    [SerializeField] private bool _thirdTowerSecondSpecLock;
    [Space]
    [SerializeField] private bool _fourthTowerFirstSpecLock;
    [SerializeField] private bool _fourthTowerSecondSpecLock;
    [Space]
    [Header("Images")]
    [SerializeField] private GameObject _imageHighSpeed;
    [SerializeField] private GameObject _imagelowSpeed;
    [Space]
    [SerializeField] private GameObject _firstAccept;
    [SerializeField] private GameObject _secondAccept;
    [SerializeField] private GameObject _thirdAccept;
    [SerializeField] private GameObject _fourthAccept;
    [Space]
    [SerializeField] private GameObject _lockImage;
    [SerializeField] private GameObject _upgradeImage;
    [SerializeField] private GameObject _sellAcceptImage;
    [SerializeField] private GameObject _sellAcceptSpecImage;
    [Space]
    [SerializeField] private Image _imageFirstSpecUpgrade;
    [SerializeField] private Image _imageSecondSpecUpgrade;
    [Space]
    [SerializeField] private Sprite _imageFirstTowerFirstSpec;
    [SerializeField] private Sprite _imageFirstTowerSecondSpec;
    [Space]
    [SerializeField] private Sprite _imageSecondTowerFirstSpec;
    [SerializeField] private Sprite _imageSecondTowerSecondSpec;
    [Space]
    [SerializeField] private Sprite _imageThirdTowerFirstSpec;
    [SerializeField] private Sprite _imageThirdTowerSecondSpec;
    [Space]
    [SerializeField] private Sprite _imageFourthTowerFirstSpec;
    [SerializeField] private Sprite _imageFourthTowerSecondSpec;
    [Space]
    [SerializeField] private GameObject _imageFirstSpecLock;
    [SerializeField] private GameObject _imageSecondSpecLock;
    [Space]
    [Space]
    [Header("Gameobjects")]
    [SerializeField] private Camera _camera;
    [SerializeField] private GameObject _allocation;
    [SerializeField] private GameObject _allocationMap;
    [Space]
    [Header("Frames")]
    [SerializeField] private GameObject _frameShopButton;
    [SerializeField] private GameObject _frameUpgradeButton;
    [SerializeField] private GameObject _frameUpgradeSpecButton;
    [SerializeField] private GameObject _largeButton;
    [Space]
    [Space]
    [Header("Characteristics frames")]
    [SerializeField] private GameObject _frameCharacteristics;
    [SerializeField] private GameObject _frameSpecCharacteristics;
    [Space]
    [SerializeField] private TextMeshProUGUI _textDeafaultCharacteristics;
    [SerializeField] private TextMeshProUGUI _textSpecCharacteristics;
    [Space]
    [Space]
    [Header("Preview")]
    [SerializeField] private GameObject _minigunPreview;
    [SerializeField] private GameObject _twiinPreview;
    [SerializeField] private GameObject _gravityPreview;
    [SerializeField] private GameObject _railPreview;
    [Space]
    [Header("Range")]
    [SerializeField] private RangeTower _minigunRange;
    [SerializeField] private RangeTower _twiinRange;
    [SerializeField] private RangeTower _gravityRange;
    [SerializeField] private RangeTower _railRange;
    [Space]
    [Space]
    [Header("Towers")]
    [SerializeField] private GameObject _firstTowerObj;
    [SerializeField] private GameObject _secondTowerObj;
    [SerializeField] private GameObject _thirdTowerObj;
    [SerializeField] private GameObject _fourthTowerObj;
    [Space]
    [Space]
    [Header("Text")]
    [SerializeField] private TextMeshProUGUI _moneyText;
    [SerializeField] private TextMeshProUGUI _textUpgradeCost;
    [SerializeField] private TextMeshProUGUI _textSellCost;
    [SerializeField] private TextMeshProUGUI _textSpecSellCost;
    [SerializeField] private TextMeshProUGUI _firstPrice;
    [SerializeField] private TextMeshProUGUI _secondPrice;
    [SerializeField] private TextMeshProUGUI _thirdPrice;
    [SerializeField] private TextMeshProUGUI _fourthPrice;
    [Space]
    [SerializeField] public TextMeshProUGUI _firstSpecPriceText;
    [SerializeField] public TextMeshProUGUI _secondSpecPriceText;
    [Space]
    [Header("Layers")]
    [SerializeField] private LayerMask _groundLayer;
    [Space]
    [Header("Buttons")]
    [SerializeField] private Button _firstBuyButton;
    [SerializeField] private Button _secondBuyButton;
    [SerializeField] private Button _thirdBuyButton;
    [SerializeField] private Button _fourthBuyButton;
    [SerializeField] private Button _upgradeButton;
    [SerializeField] private Button _firstSpecUpgradeButton;
    [SerializeField] private Button _secondSpecUpgradeButton;
    [SerializeField] private Button _buttonChangeSpeed;

    private List<int> _uniqueIndexes = new List<int>();

    private bool _isHighSpeed;
    private bool _showAllocationMapStarted;
    private bool _firstTowerWasBuilt;
    private bool _isAcceptedSell;
    private bool _blockFirstSpecBuyButton;
    private bool _blockSecondSpecBuyButton;

    private RaycastHit2D _raycastHit;

    private Vector3 _mousePos;
    private Vector3 _tmpPos_frame;
    private Vector3 _tmpPos_allocation;

    private GameObject _tmp = null;
    private GameObject _tmpTowerObj;
    private GameObject _firstTowerPreview;
    private GameObject _secondTowerPreview;
    private GameObject _thirdTowerPreview;
    private GameObject _fourthTowerPreview;

    private Tower _firstTower;
    private Tower _secondTower;
    private Tower _thirdTower;
    private Tower _fourthTower;

    private Tower _tmpTower;
    private GameObject _tmpTowerGameObject;

    private Cell _cell;

    private int _price = 0;
    private int _towerSelected = -1;
    private float _moneyCount;

    public bool FirstTowerWasBuilt => _firstTowerWasBuilt;

    private void Start()
    {

        Application.targetFrameRate = 60;

        int coinBuff = 0;

        if (PlayerPrefs.GetInt("StarBuff0") == 1)
        {
            coinBuff += 10;
        }
        if (PlayerPrefs.GetInt("StarBuff2") == 1)
        {
            coinBuff += 15;
        }

        float startCoins = _startMoney;

        startCoins = startCoins + (startCoins / 100) * coinBuff;

        AddMoney((int)startCoins);

        CloseShop();
        CloseUpgrade();
        CloseSpecUpgrade();

        _allocationMap.SetActive(false);

        _firstTower = _firstTowerObj.GetComponent<Tower>();
        _secondTower = _secondTowerObj.GetComponent<Tower>();
        _thirdTower = _thirdTowerObj.GetComponent<Tower>();
        _fourthTower = _fourthTowerObj.GetComponent<Tower>();

        //_firstPrice.text = _firstTower.L1Price.ToString();
        //_secondPrice.text = _secondTower.L1Price.ToString();
        //_thirdPrice.text = _thirdTower.L1Price.ToString();
        //_fourthPrice.text = _fourthTower.L1Price.ToString();

        if (_stage == TowerStage.classic)
        {
            _firstTowerPreview = _minigunPreview;
            _secondTowerPreview = _twiinPreview;
            _thirdTowerPreview = _gravityPreview;
            _fourthTowerPreview = _railPreview;
        }

        DisableAllTowerPreview();
    }

    public bool IsPointerOverUIElement()
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = Input.mousePosition;

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);
        return results.Count > 0;
    }

    private void Update()
    {
        if (/*!EventSystem.current.IsPointerOverGameObject()*/true)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (IsPointerOverUIElement())
                {
                    return;
                }

                CloseShop();
                CloseSpecUpgrade();
                CloseUpgrade();

                _mousePos = _camera.ScreenToWorldPoint(Input.mousePosition);
                _raycastHit = Physics2D.Raycast(_mousePos, Vector2.zero, 1000, _groundLayer);
                Debug.DrawRay(_mousePos, Vector2.up, Color.magenta, 2f);

                if (_raycastHit)
                {
                    _cell = _raycastHit.collider.gameObject.GetComponent<Cell>();

                    _tmpPos_allocation = _cell.transform.position;
                    _tmpPos_frame = _tmpPos_allocation;

                    if (_cell.transform.position.y >= 6)
                    {
                        _tmpPos_frame.y -= 2.5f;
                    }
                    else
                    {
                        _tmpPos_frame.y += 2.5f;
                    }

                    if (_cell.CompareTag("Ground"))
                    {
                        OpenShop();

                        _frameShopButton.transform.position = _tmpPos_frame;
                        _allocation.transform.position = _tmpPos_allocation;

                        EventBus.onCellSelected?.Invoke();
                    }
                    else if (_cell.CompareTag("Ground_with_tower"))
                    {
                        _tmpTower = _cell.GetTower();
                        _tmpTowerGameObject = _cell.GetTowerGameObject();

                        //if (_tmpTower.CurrLevel == 2 && !_tmpTower.HasFirstSpec && !_tmpTower.HasSecondSpec)
                        //{
                        //    OpenSpecUpgrade();
                        //}
                        //else
                        //{
                        //    OpenUpgrade();
                        //}

                        ShowTowerInfo();

                        _frameUpgradeButton.transform.position = _tmpPos_frame;
                        _frameUpgradeSpecButton.transform.position = _tmpPos_frame;
                        _allocation.transform.position = _tmpPos_allocation;
                    }
                    else if (_cell.CompareTag("Ground_far"))
                    {
                        if (!_showAllocationMapStarted)
                        {
                            StartCoroutine(ShowAllocationMap());
                        }
                    }
                }
            }
        }
    }

    private void FixedUpdate()
    {
        if (_tmpTower != null)
        {
            if (_frameUpgradeButton.activeSelf)
            {
                //if (_tmpTower.CurrPrice > _moneyCount || _tmpTower.CurrLevel == 4)
                //{
                //    _upgradeButton.interactable = false;
                //}
                //else
                //{
                //    _upgradeButton.interactable = true;
                //}
            }

            //if (_frameUpgradeSpecButton)
            //{
            //    if (_tmpTower.CurrLevel == 2 && _tmpTower.L3FirstSpecPrice > _moneyCount || _blockFirstSpecBuyButton)
            //    {
            //        _firstSpecUpgradeButton.interactable = false;
            //    }
            //    else
            //    {
            //        _firstSpecUpgradeButton.interactable = true;
            //    }

            //    if (_tmpTower.CurrLevel == 2 && _tmpTower.L3SecondSpecPrice > _moneyCount || _blockSecondSpecBuyButton)
            //    {
            //        _secondSpecUpgradeButton.interactable = false;
            //    }
            //    else
            //    {
            //        _secondSpecUpgradeButton.interactable = true;
            //    }
            //}
        }

        //if (_frameShopButton.activeSelf)
        //{
        //    if (_firstTower.L1Price > _moneyCount)
        //    {
        //        _firstBuyButton.interactable = false;
        //    }
        //    else
        //    {
        //        _firstBuyButton.interactable = true;
        //    }

        //    if (_secondTower.L1Price > _moneyCount)
        //    {
        //        _secondBuyButton.interactable = false;
        //    }
        //    else
        //    {
        //        _secondBuyButton.interactable = true;
        //    }

        //    if (_thirdTower.L1Price > _moneyCount)
        //    {
        //        _thirdBuyButton.interactable = false;
        //    }
        //    else
        //    {
        //        _thirdBuyButton.interactable = true;
        //    }

        //    if (_fourthTower.L1Price > _moneyCount)
        //    {
        //        _fourthBuyButton.interactable = false;
        //    }
        //    else
        //    {
        //        _fourthBuyButton.interactable = true;
        //    }
        //}
    }

    private void TakeMoney(int num)
    {
        _moneyCount -= num;
        _moneyText.text = _moneyCount.ToString();
    }

    private void AddMoney(float num)
    {
        _moneyCount += num;
        _moneyText.text = _moneyCount.ToString();
    }

    private IEnumerator ShowAllocationMap()
    {
        _showAllocationMapStarted = true;
        _allocationMap.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        _allocationMap.SetActive(false);
        _showAllocationMapStarted = false;
    }

    public void CloseShop()
    {
        _allocation.SetActive(false);
        _frameShopButton.SetActive(false);
        _largeButton.SetActive(false);

        CloseTowerInfo();
        DisableAllTowerPreview();
    }

    public void OpenShop()
    {
        _allocation.SetActive(true);
        _frameShopButton.SetActive(true);
        //_largeButton.SetActive(true);

        EventSystem.current.SetSelectedGameObject(_frameShopButton);
    }

    public void CloseUpgrade()
    {
        _isAcceptedSell = false;

        _allocation.SetActive(false);
        _frameUpgradeButton.SetActive(false);
        _largeButton.SetActive(false);
        _sellAcceptImage.SetActive(false);
        _sellAcceptSpecImage.SetActive(false);

        CloseTowerInfo();
        DisableAllTowerPreview();
    }

    public void OpenUpgrade()
    {
        //if (_tmpTower.CurrLevel == 4)
        //{
        //    _textUpgradeCost.text = "max";
        //    _lockImage.SetActive(true);
        //    _upgradeImage.SetActive(false);
        //}
        //else
        //{
        //    _textUpgradeCost.text = _tmpTower.CurrPrice.ToString();
        //    _lockImage.SetActive(false);
        //    _upgradeImage.SetActive(true);
        //}

        //int sellcost = _tmpTower.CurrSellCost;

        //if (PlayerPrefs.GetInt("StarBuff1") == 1)
        //{
        //    sellcost *= 2;
        //}

        //_textSellCost.text = sellcost.ToString();

        //_allocation.SetActive(true);
        //_frameUpgradeButton.SetActive(true);
        //_frameUpgradeSpecButton.SetActive(false);
        ////_largeButton.SetActive(true);

        //ShowTowerInfo();

        //EventSystem.current.SetSelectedGameObject(_frameUpgradeButton);
    }

    public void CloseSpecUpgrade()
    {
        _isAcceptedSell = false;

        _frameUpgradeSpecButton.SetActive(false);
        _largeButton.SetActive(false);
        _allocation.SetActive(false);
        _sellAcceptImage.SetActive(false);
        _sellAcceptSpecImage.SetActive(false);

        CloseTowerInfo();
        DisableAllTowerPreview();
    }

    public void OpenSpecUpgrade()
    {
        //_firstSpecPriceText.text = _tmpTower.L3FirstSpecPrice.ToString();
        //_secondSpecPriceText.text = _tmpTower.L3SecondSpecPrice.ToString();

        int sellcost = 0; /*_tmpTower.L2SellCost;*/

        if (PlayerPrefs.GetInt("StarBuff1") == 1)
        {
            sellcost *= 2;
        }

        _textSpecSellCost.text = sellcost.ToString();

        TowerType towerType = _tmpTower.TowerType;

        if (towerType == TowerType.Minigun)
        {
            if (_imageFirstSpecLock != null)
            {
                if (_firstTowerFirstSpecLock)
                {
                    _blockFirstSpecBuyButton = true;
                    _imageFirstSpecLock.SetActive(true);
                }
                else
                {
                    _blockFirstSpecBuyButton = false;
                    _imageFirstSpecLock.SetActive(false);
                }
            }

            if (_imageSecondSpecLock != null)
            {
                if (_firstTowerSecondSpecLock)
                {
                    _blockSecondSpecBuyButton = true;
                    _imageSecondSpecLock.SetActive(true);
                }
                else
                {
                    _blockSecondSpecBuyButton = false;
                    _imageSecondSpecLock.SetActive(false);
                }
            }

            _imageFirstSpecUpgrade.sprite = _imageFirstTowerFirstSpec;
            _imageSecondSpecUpgrade.sprite = _imageFirstTowerSecondSpec;
        }
        else if (towerType == TowerType.Twiin)
        {
            if (_imageFirstSpecLock != null)
            {
                if (_secondTowerFirstSpecLock)
                {
                    _blockFirstSpecBuyButton = true;
                    _imageFirstSpecLock.SetActive(true);
                }
                else
                {
                    _blockFirstSpecBuyButton = false;
                    _imageFirstSpecLock.SetActive(false);
                }
            }

            if (_imageSecondSpecLock != null)
            {
                if (_secondTowerSecondSpecLock)
                {
                    _blockSecondSpecBuyButton = true;
                    _imageSecondSpecLock.SetActive(true);
                }
                else
                {
                    _blockSecondSpecBuyButton = false;
                    _imageSecondSpecLock.SetActive(false);
                }
            }

            _imageFirstSpecUpgrade.sprite = _imageSecondTowerFirstSpec;
            _imageSecondSpecUpgrade.sprite = _imageSecondTowerSecondSpec;
        }
        else if (towerType == TowerType.Gravity)
        {
            if (_imageFirstSpecLock != null)
            {
                if (_thirdTowerFirstSpecLock)
                {
                    _blockFirstSpecBuyButton = true;
                    _imageFirstSpecLock.SetActive(true);
                }
                else
                {
                    _blockFirstSpecBuyButton = false;
                    _imageFirstSpecLock.SetActive(false);
                }
            }

            if (_imageSecondSpecLock != null)
            {
                if (_thirdTowerSecondSpecLock)
                {
                    _blockSecondSpecBuyButton = true;
                    _imageSecondSpecLock.SetActive(true);
                }
                else
                {
                    _blockSecondSpecBuyButton = false;
                    _imageSecondSpecLock.SetActive(false);
                }
            }

            _imageFirstSpecUpgrade.sprite = _imageThirdTowerFirstSpec;
            _imageSecondSpecUpgrade.sprite = _imageThirdTowerSecondSpec;
        }
        else if (towerType == TowerType.Rail)
        {
            if (_imageFirstSpecLock != null)
            {
                if (_fourthTowerFirstSpecLock)
                {
                    _blockFirstSpecBuyButton = true;
                    _imageFirstSpecLock.SetActive(true);
                }
                else
                {
                    _blockFirstSpecBuyButton = false;
                    _imageFirstSpecLock.SetActive(false);
                }
            }

            if (_imageSecondSpecLock != null)
            {
                if (_fourthTowerSecondSpecLock)
                {
                    _blockSecondSpecBuyButton = true;
                    _imageSecondSpecLock.SetActive(true);
                }
                else
                {
                    _blockSecondSpecBuyButton = false;
                    _imageSecondSpecLock.SetActive(false);
                }
            }

            _imageFirstSpecUpgrade.sprite = _imageFourthTowerFirstSpec;
            _imageSecondSpecUpgrade.sprite = _imageFourthTowerSecondSpec;
        }

        _allocation.SetActive(true);
        _frameUpgradeSpecButton.SetActive(true);
        _frameUpgradeButton.SetActive(false);
        //_largeButton.SetActive(true);

        ShowTowerInfo();

        EventSystem.current.SetSelectedGameObject(_frameUpgradeButton);
    }

    public void ChangeSpeedByScaleTime()
    {
        if (!_isHighSpeed)
        {
            Time.timeScale *= _speedMultiply;
            _imageHighSpeed.SetActive(true);
            _imagelowSpeed.SetActive(false);
            _isHighSpeed = true;
        }
        else
        {
            Time.timeScale = 1;
            _imageHighSpeed.SetActive(false);
            _imagelowSpeed.SetActive(true);
            _isHighSpeed = false;
        }

        EventSystem.current.SetSelectedGameObject(_allocation);
    }

    private void DisableAllTowerPreview()
    {
        if (_firstTowerPreview != null)
        {
            _firstTowerPreview.SetActive(false);
            _secondTowerPreview.SetActive(false);
            _thirdTowerPreview.SetActive(false);
            _fourthTowerPreview.SetActive(false);
        }

        _firstAccept.SetActive(false);
        _secondAccept.SetActive(false);
        _thirdAccept.SetActive(false);
        _fourthAccept.SetActive(false);

        _tmp = null;

        _towerSelected = -1;
    }

    private void CloseTowerInfo()
    {
        _frameCharacteristics.SetActive(false);
        _frameSpecCharacteristics.SetActive(false);

        _minigunRange.gameObject.SetActive(false);
        _twiinRange.gameObject.SetActive(false);
        _gravityRange.gameObject.SetActive(false);
        _railRange.gameObject.SetActive(false);
    }

    private void ShowTowerInfo()
    {
        TowerType towerType = _tmpTower.TowerType;
        Vector2 tmpPos;

        tmpPos = _cell.transform.position;

        if (_cell.transform.position.y >= 6)
        {
            tmpPos.y -= 4.6f;
        }
        else
        {
            tmpPos.y -= 1.8f;
        }

        _frameCharacteristics.transform.position = tmpPos;
        _frameCharacteristics.SetActive(true);

        //if (_tmpTower.HasFirstSpec || _tmpTower.HasSecondSpec)
        //{
        //    tmpPos.y -= 1.5f;
        //    _frameSpecCharacteristics.transform.position = tmpPos;
        //    _frameSpecCharacteristics.SetActive(true);
        //}

        if (towerType == TowerType.Minigun)
        {
            Minigun tmpMinigun = _tmpTowerGameObject.GetComponent<Minigun>();
            string str = "";

            _minigunRange.gameObject.SetActive(true);
            _minigunRange.transform.position = _tmpTower.transform.position;
            _minigunRange.SetScale(tmpMinigun.CurrMaxAtackRadius);

            string str1 = "DMG: " + tmpMinigun.CurrDamage.ToString() + "  " + "AS: " + tmpMinigun.CurrDelayBtwAtack.ToString();
            string str2 = "AP: " + (tmpMinigun.CurrArmorPiercing * 100).ToString() + "%";

            _textDeafaultCharacteristics.text = str1 + "\n" + str2;

            //if (tmpMinigun.HasFirstSpec)
            //{
            //    str = "Explosion damage: " + tmpMinigun.CurrExplosionDamage.ToString();
            //}
            //else if (tmpMinigun.HasSecondSpec)
            //{
            //    str = "Freeze stack: " + tmpMinigun.CurrFrezzeIncrement.ToString();
            //}

            _textSpecCharacteristics.text = str;
        }
        else if (towerType == TowerType.Twiin)
        {
            Twiin tmpTwiin = _tmpTowerGameObject.GetComponent<Twiin>();
            string str = "";

            _twiinRange.gameObject.SetActive(true);
            _twiinRange.transform.position = _tmpTower.transform.position;
            _twiinRange.SetScale(tmpTwiin.CurrMaxAtackRadius);

            string str1;

            if (tmpTwiin.SpecTypeTwiin == SpecTypeTwiin.TwoToOneAtack)
            {
                str1 = "DMG: " + tmpTwiin.CurrentDamageTwoOne.ToString() + "x2" + "  " + "AS: " + tmpTwiin.CurrDelayBtwAtack.ToString();
            }
            else if (tmpTwiin.SpecTypeTwiin == SpecTypeTwiin.Shard)
            {
                str1 = "DMG: " + tmpTwiin.CurrDamage.ToString() + " + " + tmpTwiin.CurrShardDamage.ToString() + "x" + tmpTwiin.CurrNumShard.ToString() + "  " + "AS: " + tmpTwiin.CurrDelayBtwAtack.ToString();
            } 
            else
            {
                str1 = "DMG: " + tmpTwiin.CurrDamage.ToString() + "  " + "AS: " + tmpTwiin.CurrDelayBtwAtack.ToString();
            }

            string str2 = "AP: " + (tmpTwiin.CurrArmorPiercing * 100).ToString() + "%";

            _textDeafaultCharacteristics.text = str1 + "\n" + str2;

            //if (tmpTwiin.HasFirstSpec)
            //{
            //    _frameSpecCharacteristics.SetActive(false);
            //}
            //else if (tmpTwiin.HasSecondSpec)
            //{
            //    str = "Num shard: " + tmpTwiin.CurrNumShard.ToString();
            //}

            _textSpecCharacteristics.text = str;
        }
        else if (towerType == TowerType.Gravity)
        {
            Gravity tmpGravity = _tmpTowerGameObject.GetComponent<Gravity>();
            string str = "";

            _gravityRange.gameObject.SetActive(true);
            _gravityRange.transform.position = _tmpTower.transform.position;
            _gravityRange.SetScale(tmpGravity.CurrRange);

            _textDeafaultCharacteristics.text = "Speed: -" + (tmpGravity.CurrSpeedDivisor * 100).ToString() + " %";

            //if (tmpGravity.HasFirstSpec)
            //{
            //    str = "Money: " + tmpGravity.CurrMoneyDropMultiplier.ToString() + "x";
            //}
            //else if (tmpGravity.HasSecondSpec)
            //{
            //    if (tmpGravity.CurrLevel == 3)
            //    {
            //        str = "HP: -25%";
            //    }
            //    else if (tmpGravity.CurrLevel == 4)
            //    {
            //        str = "HP: -50%";
            //    }
            //}

            _textSpecCharacteristics.text = str;
        }
        else if (towerType == TowerType.Rail)
        {
            Rail tmpRail = _tmpTowerGameObject.GetComponent<Rail>();
            string str = "";


            _railRange.gameObject.SetActive(true);
            _railRange.transform.position = _tmpTower.transform.position;
            _railRange.SetScale(tmpRail.CurrMaxAtackRadius);

            string str1 = "DMG: " + tmpRail.CurrDamage.ToString() + "  " + "AS: " + tmpRail.CurrDelayBtwAtack.ToString();
            string str2 = "AP: " + (tmpRail.CurrArmorPiercing * 100).ToString() + "%";

            _textDeafaultCharacteristics.text = str1 + "\n" + str2;

            //if (tmpRail.HasFirstSpec)
            //{
            //    str = "Crit ñhance: " + (tmpRail.CurrCriticalChance * 100).ToString() + " %";
            //}
            //else if (tmpRail.HasSecondSpec)
            //{
            //    str = "Armor break: " + (tmpRail.CurrDecreaseArmor * 100).ToString() + " %";
            //}

            _textSpecCharacteristics.text = str;
        }
    }

    private void SetPreviewTower(int towerType)
    {
        //if (towerType == ((int)TowerType.Minigun))
        //{
        //    _tmp = _firstTowerObj;
        //    _price = _firstTower.L1Price;
        //    _firstTowerPreview.SetActive(true);
        //    _firstAccept.SetActive(true);
        //    _firstTowerPreview.transform.position = _allocation.transform.position;
        //}
        //else if (towerType == ((int)TowerType.Twiin))
        //{
        //    _tmp = _secondTowerObj;
        //    _price = _secondTower.L1Price;
        //    _secondTowerPreview.SetActive(true);
        //    _secondAccept.SetActive(true);
        //    _secondTowerPreview.transform.position = _allocation.transform.position;
        //}
        //else if (towerType == ((int)TowerType.Gravity))
        //{
        //    _tmp = _thirdTowerObj;
        //    _price = _thirdTower.L1Price;
        //    _thirdTowerPreview.SetActive(true);
        //    _thirdAccept.SetActive(true);
        //    _thirdTowerPreview.transform.position = _allocation.transform.position;
        //}
        //else if (towerType == ((int)TowerType.Rail))
        //{
        //    _tmp = _fourthTowerObj;
        //    _price = _fourthTower.L1Price;
        //    _fourthTowerPreview.SetActive(true);
        //    _fourthAccept.SetActive(true);
        //    _fourthTowerPreview.transform.position = _allocation.transform.position;
        //}

        _towerSelected = towerType;
    }

    public void PlaceTower(int towerType)
    {
        if (_cell.CompareTag("Ground"))
        {
            if (towerType != _towerSelected)
            {
                DisableAllTowerPreview();
                SetPreviewTower(towerType);
            }
            else
            {
                if (_tmp != null && _price > 0)
                {
                    _tmpTowerObj = Instantiate(_tmp, _allocation.transform.position, Quaternion.identity);
                    TakeMoney(_price);

                    int tmp = _uniqueIndexes.Count;

                    _tmpTowerObj.GetComponent<Tower>().SetUniqueTowerIndex(tmp);
                    _uniqueIndexes.Add(tmp);

                    _cell.SetTower(_tmpTowerObj);
                    CloseShop();
                }

                if (!_firstTowerWasBuilt)
                {
                    EventBus.FirstTowerWasBuilt?.Invoke();
                    _firstTowerWasBuilt = true;
                }

                DisableAllTowerPreview();
            }
        }
    }

    public void SellTower()
    {
        if (!_isAcceptedSell)
        {
            _isAcceptedSell = true;
            _sellAcceptImage.SetActive(true);
            _sellAcceptSpecImage.SetActive(true);
        }
        else
        {
            int sellcost = 0; /*_tmpTower.CurrSellCost;*/

            if (PlayerPrefs.GetInt("StarBuff1") == 1)
            {
                sellcost *= 2;
            }

            AddMoney(sellcost);
            _cell.tag = "Ground";
            Destroy(_tmpTowerGameObject);
            _sellAcceptImage.SetActive(false);
            _sellAcceptSpecImage.SetActive(false);
            CloseUpgrade();
            _isAcceptedSell = false;

            _frameUpgradeButton.SetActive(false);
            _frameUpgradeSpecButton.SetActive(false);
        }
    }

    public void UpgradeTower()
    {
        _sellAcceptImage.SetActive(false);
        int tmp = 0;  /*_tmpTower.CurrPrice;*/
        bool succes = false;

        if (_tmpTower.TowerType == TowerType.Minigun)
        {
            succes = _tmpTowerGameObject.GetComponent<Minigun>().Upgrade();
        }
        else if (_tmpTower.TowerType == TowerType.Twiin)
        {
            succes = _tmpTowerGameObject.GetComponent<Twiin>().Upgrade();
        }
        else if (_tmpTower.TowerType == TowerType.Gravity)
        {
            succes = _tmpTowerGameObject.GetComponent<Gravity>().Upgrade();
        }
        else if (_tmpTower.TowerType == TowerType.Rail)
        {
            succes = _tmpTowerGameObject.GetComponent<Rail>().Upgrade();
        }

        if (succes)
        {
            TakeMoney(tmp);
            ShowTowerInfo();

            if (true/*_tmpTower.CurrLevel == 2 && !_tmpTower.HasFirstSpec && !_tmpTower.HasSecondSpec*/)
            {
                CloseSpecUpgrade();
                OpenSpecUpgrade();
            }
            else
            {
                CloseUpgrade();
                OpenUpgrade();
            }
        }
    }

    public void UpgradeSpec(int index)
    {
        int tmp = _tmpTower.GetSpecPrice(index);

        if (_tmpTower.TowerType == TowerType.Minigun)
        {
            _tmpTowerGameObject.GetComponent<Minigun>().SetSpecType(index);
        }
        else if (_tmpTower.TowerType == TowerType.Twiin)
        {
            _tmpTowerGameObject.GetComponent<Twiin>().SetSpecType(index);
        }
        else if (_tmpTower.TowerType == TowerType.Gravity)
        {
            _tmpTowerGameObject.GetComponent<Gravity>().SetSpecType(index);
        }
        else if (_tmpTower.TowerType == TowerType.Rail)
        {
            _tmpTowerGameObject.GetComponent<Rail>().SetSpecType(index);
        }

        TakeMoney(tmp);
        ShowTowerInfo();
        CloseSpecUpgrade();
        OpenUpgrade();
    }

    private void OnEnable()
    {
        EventBus.AddMoney += AddMoney;
    }

    private void OnDisable()
    {
        EventBus.AddMoney -= AddMoney;
    }
}

enum TowerStage
{
    classic,
    laser,
}