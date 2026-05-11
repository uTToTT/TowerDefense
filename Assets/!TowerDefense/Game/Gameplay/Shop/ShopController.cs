using System;
using System.Linq;
using TToTT.TowerDefense.Economy;
using TToTT.TowerDefense.Shop;
using TToTT.TowerDefense.Towers;
using TToTT.TowerDefense.UI.Button;
using UnityEngine;

public class ShopController : IDisposable
{
    private readonly ILogger _logger;

    private readonly EconomyController _economy;
    private readonly DragAndDropController _dragAndDrop;
    private readonly TowerManager _towerManager;
    private readonly MapObjectPreviewFactoryRegistry _previewFactory;
    private readonly ShopConfig _config;
    private readonly IUIButton _reroll;

    private ProductSlot[] _slots;
    private ProductSlot _activeSlot;

    private int _currRerollCost;
    private float _totalWeight;

    public ShopController(
        EconomyController economy,
        DragAndDropController dragAndDrop,
        TowerManager towerManager,
        MapObjectPreviewFactoryRegistry previewFactory,
        ShopConfig config,
        ShopSlots slots,
        ButtonRegistry buttons,
        ILogger logger)
    {
        _economy = economy;
        _dragAndDrop = dragAndDrop;
        _towerManager = towerManager;
        _previewFactory = previewFactory;
        _config = config;
        _slots = slots.Slots;
        _reroll = buttons.Get(ButtonId.Reroll);
        _logger = logger;

        _previewFactory.Init();
        _totalWeight = CalculateTotalWeight();
        _economy.AddMoney(_config.StartBalance);

        foreach (var slot in _slots)
        {
            slot.OnDragPerformed += HandleDragStarted;
            slot.OnDragCanceled += HandleDragCanceled;
        }

        _reroll.OnClick += TryReroll;
       
    }

    public void Restart()
    {
        _currRerollCost = _config.StartRerollCost;
        Reroll(free: true);
    }

    public void TryReroll()
    {
        Reroll(free: false);
    }

    private void Reroll(bool free)
    {
        if (!free)
        {
            if (!_economy.CanSpend(_currRerollCost)) return;
            _economy.Spend(_currRerollCost);
            _currRerollCost += _config.RerollDelta;
        }

        ClearSlots();

        foreach (var slot in _slots)
        {
            var product = PickWeighted();
            var preview = _previewFactory.Create(product.ProductType);

            if (preview is TowerPreview tower)
            {
                tower.Disable();
            }

            slot.SetProduct(preview, product);
        }
    }

    private void HandleDragStarted(ProductSlot slot)
    {
        if (!_economy.CanSpend(slot.ProductConfig.Cost))
        {
#if UNITY_EDITOR
            _logger.Log("Not enough money!");
#endif
            return;
        }

        _dragAndDrop.OnDropSuccess += HandleDropSuccess;
        _dragAndDrop.OnDropFailed += HandleDropFailed;

        _activeSlot = slot;
        _dragAndDrop.BeginDrag(slot.MapObject);
    }

    private void HandleDragCanceled(ProductSlot slot)
    {
        _dragAndDrop.EndDrag();
    }

    private void HandleDropSuccess(MapObject preview, MapObject obj, Vector2Int mapPos)
    {
        _previewFactory.Return(preview);

        if (obj is Tower tower)
        {
            tower.HideRange();
            tower.Enable();
            _towerManager.Register(tower);
        }

        _economy.Spend(_activeSlot.ProductConfig.Cost);
        _activeSlot.Clear();
        _activeSlot = null;

        if (_slots.All(s => s.IsEmpty))
            Reroll(free: true);

        _dragAndDrop.OnDropSuccess -= HandleDropSuccess;
        _dragAndDrop.OnDropFailed -= HandleDropFailed;
    }

    private void HandleDropFailed(MapObject preview)
    {
        if (_activeSlot != null)
            preview.transform.position = _activeSlot.transform.position;

        _activeSlot = null;

        _dragAndDrop.OnDropSuccess -= HandleDropSuccess;
        _dragAndDrop.OnDropFailed -= HandleDropFailed;
    }

    private void ClearSlots()
    {
        foreach (var slot in _slots)
        {
            if (slot.MapObject != null)
            {
                _previewFactory.Return(slot.MapObject);
                slot.Clear();
            }
        }
    }

    private ProductConfig PickWeighted()
    {
        float r = UnityEngine.Random.value * _totalWeight;
        float cumulative = 0f;
        foreach (var product in _config.Products)
        {
            cumulative += product.Weight;
            if (r <= cumulative) return product;
        }
        return _config.Products[^1];
    }

    private float CalculateTotalWeight()
    {
        float total = 0f;
        foreach (var p in _config.Products)
            total += p.Weight;
        return total;
    }

    public void Dispose()
    {
        foreach (var slot in _slots)
        {
            slot.OnDragPerformed -= HandleDragStarted;
            slot.OnDragCanceled -= HandleDragCanceled;
        }

        _reroll.OnClick -= TryReroll;
        _dragAndDrop.OnDropSuccess -= HandleDropSuccess;
        _dragAndDrop.OnDropFailed -= HandleDropFailed;
    }
}