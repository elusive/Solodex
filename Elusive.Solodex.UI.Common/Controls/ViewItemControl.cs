using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Elusive.Solodex.Core.Extensions;
using Prism.Commands;
using Prism.Regions;

namespace Elusive.Solodex.UI.Common.Controls
{
    public class ViewItemControl : Button
    {
        /// <summary>
        ///     IsSelected property identifies that the ViewItem is the currently selected view.
        /// </summary>
        public static readonly DependencyProperty IsSelectedProperty = DependencyProperty.Register(
            "IsSelected",
            typeof (bool),
            typeof (ViewItemControl),
            new PropertyMetadata(false));

        /// <summary>
        ///     ViewRegionName property identifies the region name where the view should be activated.
        ///     The default value is <see cref="RegionNames.MainRegion" />
        /// </summary>
        public static readonly DependencyProperty ViewRegionNameProperty = DependencyProperty.Register(
            "ViewRegionName",
            typeof (string),
            typeof (ViewItemControl),
            new PropertyMetadata(RegionNames.MainRegion, RegionNamePropertyChanged));

        /// <summary>
        ///     ViewType property identifies the view type that will be activated when the ViewItem is selected.
        /// </summary>
        public static readonly DependencyProperty ViewTypeProperty = DependencyProperty.Register(
            "ViewType",
            typeof (Type),
            typeof (ViewItemControl));

        /// <summary>
        ///     Prism object used for switching views in a region
        /// </summary>
        public readonly IRegionManager RegionManager;

        private List<Type> _childViews;

        private bool _didSubscribe;

        /// <summary>
        ///     Constructor
        /// </summary>
        public ViewItemControl(IRegionManager regionManager)
        {
            regionManager.RequireThat("regionManager").IsNotNull();

            RegionManager = regionManager;
            Command = new DelegateCommand<ViewItemControl>(OnActivate);
            CommandParameter = this;
            Style = FindResource("ViewItemControlStyle") as Style;
        }

        /// <summary>
        ///     Default constructor.
        ///     This is here to support the XAML designer.
        /// </summary>
        public ViewItemControl()
        {
        }

        public List<Type> ChildViews
        {
            get { return _childViews ?? (_childViews = new List<Type>()); }
            set { _childViews = value; }
        }

        public bool IsSelected
        {
            get { return (bool) GetValue(IsSelectedProperty); }
            set { SetValue(IsSelectedProperty, value); }
        }

        public string ViewRegionName
        {
            get { return (string) GetValue(ViewRegionNameProperty); }
            set { SetValue(ViewRegionNameProperty, value); }
        }

        public Type ViewType
        {
            get { return (Type) GetValue(ViewTypeProperty); }
            set { SetValue(ViewTypeProperty, value); }
        }

        public void SubscribeNavigationServiceNavigated()
        {
            if (!_didSubscribe && RegionManager.Regions.Any(x => x.Name == ViewRegionName))
            {
                var region = RegionManager.Regions[ViewRegionName];
                if (region != null)
                {
                    region.NavigationService.Navigated += NavigationServiceNavigated;
                    _didSubscribe = true;
                }
            }
        }

        private static void RegionNamePropertyChanged(
            DependencyObject dependencyObject,
            DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            var item = dependencyObject as ViewItemControl;
            if (item != null)
            {
                item.SubscribeNavigationServiceNavigated();
            }
        }

        private bool IsClassNameAMatch(string nameToMatch, Type type)
        {
            return (type.Name == nameToMatch) || (type.FullName == nameToMatch);
        }

        /// <summary>
        ///     Navigated event from NavigationService
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">args</param>
        private void NavigationServiceNavigated(object sender, RegionNavigationEventArgs e)
        {
            string pathToMatch = e.Uri.ToString();
            IsSelected = IsClassNameAMatch(pathToMatch, ViewType)
                         || ChildViews.Any(x => IsClassNameAMatch(pathToMatch, x));
        }

        /// <summary>
        ///     When ViewItemControl is clicked then activate.
        /// </summary>
        private void OnActivate(ViewItemControl viewItem)
        {
            viewItem.RequireThat().IsNotNull();
            
            SubscribeNavigationServiceNavigated();

            if (!string.IsNullOrEmpty(viewItem.ViewRegionName) && (viewItem.ViewType != null))
            {
                RegionManager.RequestNavigate(viewItem.ViewRegionName, viewItem.ViewType.Name);
            }
        }
    }
}