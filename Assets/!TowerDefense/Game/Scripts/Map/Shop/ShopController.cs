using System;
using System.Linq;
using TToTT.TowerDefense.Map;
using TToTT.TowerDefense.Towers;
using UnityEngine;

public class ShopController : IDisposable
{
    private readonly EconomyController _economy;
    private readonly DragAndDropController _dragAndDrop;
    private readonly BuildController _buildController;
    private readonly TowerManager _towerManager;
    private readonly MapObjectPreviewFactoryRegistry _previewFactory;
    private readonly ShopConfig _config; 

    private ProductSlot[] _slots;
    private ProductSlot _activeSlot;

    private int _currRerollCost;
    private float _totalWeight;

    public ShopController(
        EconomyController economy,
        DragAndDropController dragAndDrop,
        BuildController buildController,
        TowerManager towerManager,
        MapObjectPreviewFactoryRegistry previewFactory,
        ShopConfig config)
    {
        _economy = economy;
        _dragAndDrop = dragAndDrop;
        _buildController = buildController;
        _towerManager = towerManager;
        _previewFactory = previewFactory;
        _config = config;
        _totalWeight = CalculateTotalWeight();

        _dragAndDrop.OnDropSuccess += HandleDropSuccess;
        _dragAndDrop.OnDropFailed += HandleDropFailed;
    }

    public void Init(ProductSlot[] slots, ButtonWrapper rerollButton)
    {
        _slots = slots;
        rerollButton.OnClick += TryReroll;

        foreach (var slot in _slots)
        {
            slot.OnDragPerformed += HandleDragStarted;
            slot.OnDragCanceled += HandleDragCanceled;
        }
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
            slot.SetProduct(preview, product);
        }
    }

    private void HandleDragStarted(ProductSlot slot)
    {
        if (!_economy.CanSpend(slot.ProductConfig.Cost)) return;

        _activeSlot = slot;
        _dragAndDrop.BeginDrag(slot.MapObject);
    }

    private void HandleDragCanceled(ProductSlot slot)
    {
        _activeSlot = null;
        _dragAndDrop.EndDrag();
    }

    private void HandleDropSuccess(MapObject preview, Vector2Int mapPos)
    {
        _previewFactory.Return(preview);

        if (!_buildController.TryBuild(
                _activeSlot.ProductConfig.ProductType,
                mapPos,
                out var obj)) return;

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
    }

    private void HandleDropFailed(MapObject preview)
    {
        if (_activeSlot != null)
            preview.transform.position = _activeSlot.transform.position;

        _activeSlot = null;
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
        _dragAndDrop.OnDropSuccess -= HandleDropSuccess;
        _dragAndDrop.OnDropFailed -= HandleDropFailed;
    }
}