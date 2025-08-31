# load-image-from-mobile-gallery-in-.net-maui-chat
This demo explains about how to load and view the image from mobile gallery using .NET MAUI Chat(SfChat) and Popup(SfPopup).

## Sample

```xaml  

    <ContentPage.Behaviors>
        <local:PageBehavior/>
    </ContentPage.Behaviors>
    <ContentPage.Content>
        <sfchat:SfChat x:Name="sfChat"
                           Messages="{Binding Messages}"
                           CurrentUser="{Binding CurrentUser}"
                           ShowAttachmentButton="True"/>
    </ContentPage.Content>

Behavior:

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

            DataTemplate bodyTemplateView = new DataTemplate(() =>
            {
                return popupTemplate;
            });

            attachmentPopup.ContentTemplate = bodyTemplateView;

            attachmentPopup.RelativeView = chat.Editor.Parent.Parent as View;
            attachmentPopup.RelativePosition = PopupRelativePosition.AlignTop;

            // Popup to zoomin/zoomout the image when tapped it.
            imagePopup = new SfPopup();


            DataTemplate messageViewTemplate = new DataTemplate(() =>
            {
                var image = new Image();
                ...
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

```

## Requirements to run the demo

To run the demo, refer to [System Requirements for .NET MAUI](https://help.syncfusion.com/maui/system-requirements)

## Troubleshooting:
### Path too long exception

If you are facing path too long exception when building this example project, close Visual Studio and rename the repository to short and build the project.

## License

Syncfusion® has no liability for any damage or consequence that may arise from using or viewing the samples. The samples are for demonstrative purposes. If you choose to use or access the samples, you agree to not hold Syncfusion® liable, in any form, for any damage related to use, for accessing, or viewing the samples. By accessing, viewing, or seeing the samples, you acknowledge and agree Syncfusion®'s samples will not allow you seek injunctive relief in any form for any claim related to the sample. If you do not agree to this, do not view, access, utilize, or otherwise do anything with Syncfusion®'s samples.
