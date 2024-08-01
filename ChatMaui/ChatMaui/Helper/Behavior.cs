using Syncfusion.Maui.Chat;
using Syncfusion.Maui.Popup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatMaui
{
    public class PageBehavior : Behavior<ContentPage>
    {
        private SfChat chat;
        private ViewModel viewModel;
        SfPopup attachmentPopup;
        SfPopup imagePopup;

        protected override void OnAttachedTo(ContentPage bindable)
        {
            viewModel = new ViewModel();
            chat = bindable.FindByName<SfChat>("sfChat");
            bindable.BindingContext = viewModel;

            chat.AttachmentButtonClicked += OnAttachmentButtonClicked;
            chat.ImageTapped += OnChatImageTapped;

            // Attachment options view Popup
            var popupTemplate = new AttachmentPopup();
            popupTemplate.BindingContext = viewModel;
            attachmentPopup = new SfPopup();
            attachmentPopup.BindingContext = viewModel;
            attachmentPopup.ShowHeader = false;
            attachmentPopup.ShowFooter = false;
            attachmentPopup.HeightRequest = 300;
            attachmentPopup.PopupStyle.CornerRadius = 30;
            attachmentPopup.AnimationMode = PopupAnimationMode.Fade;

            attachmentPopup.SetBinding(SfPopup.IsOpenProperty, new Binding("CanShowPopup",BindingMode.TwoWay));

            DataTemplate bodyTemplateView = new DataTemplate(() =>
            {
                return popupTemplate;
            });

            attachmentPopup.ContentTemplate = bodyTemplateView;

            attachmentPopup.RelativeView = chat.Editor.Parent.Parent as View;
            attachmentPopup.RelativePosition = PopupRelativePosition.AlignTop;

            // Popup to zoomin/zoomout the image when tapped it.
            imagePopup = new SfPopup();
            imagePopup.ShowHeader = true;
            imagePopup.ShowFooter = false;
            imagePopup.IsFullScreen = true;
            imagePopup.ShowCloseButton = true;
            imagePopup.HeaderTitle = string.Empty;
            imagePopup.PopupStyle.CornerRadius = 0;
            imagePopup.Padding = new Thickness(4);
            imagePopup.AnimationMode = PopupAnimationMode.Zoom;


            DataTemplate messageViewTemplate = new DataTemplate(() =>
            {
                var image = new Image();
                var binding = new Binding("Source", BindingMode.Default);
                image.SetBinding(Image.SourceProperty, binding);
                return image;
            });

            imagePopup.ContentTemplate = messageViewTemplate;

            base.OnAttachedTo(bindable);
        }

        private void OnChatImageTapped(object? sender, ImageTappedEventArgs e)
        {
            if (e.Message is ImageMessage)
            {
                imagePopup.BindingContext = e.Message;
                imagePopup.Show();
            }
        }

        private void OnAttachmentButtonClicked(object? sender, EventArgs e)
        {
            viewModel.CanShowPopup = true;
        }

        protected override void OnDetachingFrom(ContentPage bindable)
        {
            chat.AttachmentButtonClicked -= OnAttachmentButtonClicked;
            chat.ImageTapped -= OnChatImageTapped;
            chat = null;
            viewModel = null;
            attachmentPopup = null;
            imagePopup = null;
            base.OnDetachingFrom(bindable);
        }
    }
}
