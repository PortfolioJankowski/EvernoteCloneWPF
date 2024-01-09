using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using EvernoteCloneWPF.ViewModel.Helper;
using EvernoteCloneWPF.ViewModel.VM;
using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace EvernoteCloneWPF.View
{
    /// <summary>
    /// Logika interakcji dla klasy NotesWindow.xaml
    /// </summary>
    public partial class NotesWindow : Window
    {
        NotesVM viewModel;
        public NotesWindow()
        {
            InitializeComponent();
            var fontFamilies = Fonts.SystemFontFamilies.OrderBy(f => f.Source);
            fontFamilyComboBox.ItemsSource = fontFamilies;

            List<double> fontSizes = new List<double>() { 8, 10, 12, 14, 16, 20, 24, 28, 48 };
            fontSizeComboBox.ItemsSource = fontSizes;

            viewModel = Resources["vm"] as NotesVM;
            viewModel.SelectedNoteChanged += ViewModel_SelectedNoteChanged;
        }

        //nadpisuje event aktywacyjny w pierwszym oknie które się odpala
        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);

            if (string.IsNullOrEmpty(App.UserId))
            {
                //na początku UserId zainicjalizowane w APP jest równe empty stringowi, dlatego będzie odpalać
                //login window
                LoginWindow loginWindow = new LoginWindow();
                loginWindow.ShowDialog();
            }
        }
        private async void ViewModel_SelectedNoteChanged(object? sender, EventArgs e)
        {
            contentRichTextBox.Document.Blocks.Clear();
            if(viewModel.SelectedNote != null)
            {
                //notatka się zmienia -> jeśli jest Filelocation to
                if (!string.IsNullOrEmpty(viewModel.SelectedNote.FileLocation))
                {
                    //będę chciał pobrać sobie tą notatke z Azura na kompa, więc tworze sobie ścieżkę
                    string downloadPath = $"{viewModel.SelectedNote.Id}.rtf";
                    //pobieramy z Azure
                    await new BlobClient(new Uri(viewModel.SelectedNote.FileLocation)).DownloadToAsync(downloadPath);
                    using (FileStream fileStream = new FileStream(downloadPath, FileMode.Open))
                    {   
                        //otwieram sobie plik i przypisuje sobie do textranga w edytorze 
                        var content = new TextRange(contentRichTextBox.Document.ContentStart, contentRichTextBox.Document.ContentEnd);
                        content.Load(fileStream, DataFormats.Rtf);
                    } ;
                    
                } 
            }
            
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            string region = "southcentralus";
            string key = "67caccc4ec5642db9f24b70b07dc6504";

            var speechConfig = SpeechConfig.FromSubscription(key, region);
            using(var audioConfig = AudioConfig.FromDefaultMicrophoneInput())
            {
                using (var recognizer = new SpeechRecognizer(speechConfig, audioConfig))
                {
                    var result = await recognizer.RecognizeOnceAsync();
                    contentRichTextBox.Document.Blocks.Add(new Paragraph(new Run(result.Text))); 
                }
            }
        }

        private void contentRichTextBox_TextChanged(object sender, RoutedEventArgs e)
        {
            int ammountofCharacters = new TextRange(contentRichTextBox.Document.ContentStart, contentRichTextBox.Document.ContentEnd).Text.Length;
            statusTextBlock.Text = $"Document length: {ammountofCharacters}";
        }

        private void BoldButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as ToggleButton;
            // ?? -> jezeli null to fałsz
            bool isButtonChecked = button.IsChecked ?? false;

            if (isButtonChecked)
                contentRichTextBox.Selection.ApplyPropertyValue(Inline.FontWeightProperty, FontWeights.Bold);
            else
                contentRichTextBox.Selection.ApplyPropertyValue(Inline.FontWeightProperty, FontWeights.Normal);
        }

        private void contentRichTextBox_SelectionChanged(object sender, RoutedEventArgs e)
        {
            var selectedWeight = contentRichTextBox.Selection.GetPropertyValue(Inline.FontWeightProperty);
            BoldButton.IsChecked = (selectedWeight != DependencyProperty.UnsetValue) && selectedWeight.Equals(FontWeights.Bold);

            var selectedStyle = contentRichTextBox.Selection.GetPropertyValue(Inline.FontStyleProperty);
            BoldButton.IsChecked = (selectedStyle != DependencyProperty.UnsetValue) && selectedStyle.Equals(FontStyles.Italic);

            var selectedDecoration = contentRichTextBox.Selection.GetPropertyValue(Inline.TextDecorationsProperty);
            BoldButton.IsChecked = (selectedDecoration != DependencyProperty.UnsetValue) && selectedDecoration.Equals(TextDecorations.Underline);

            fontFamilyComboBox.SelectedItem = contentRichTextBox.Selection.GetPropertyValue(Inline.FontFamilyProperty);
            fontSizeComboBox.Text = (contentRichTextBox.Selection.GetPropertyValue(Inline.FontSizeProperty)).ToString();
        }

        private void UnderlineButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as ToggleButton;
            // ?? -> jezeli null to fałsz
            bool isButtonChecked = button.IsChecked ?? false;

            if (isButtonChecked)
                contentRichTextBox.Selection.ApplyPropertyValue(Inline.TextDecorationsProperty, TextDecorations.Underline);
            else 
            {
                TextDecorationCollection textDecorations;
                (contentRichTextBox.Selection.GetPropertyValue(Inline.TextDecorationsProperty) as TextDecorationCollection).TryRemove(TextDecorations.Underline, out textDecorations);
                contentRichTextBox.Selection.ApplyPropertyValue(Inline.TextDecorationsProperty, textDecorations);
            }       
        }

        private void ItalicButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as ToggleButton;
            // ?? -> jezeli null to fałsz
            bool isButtonChecked = button.IsChecked ?? false;

            if (isButtonChecked)
                contentRichTextBox.Selection.ApplyPropertyValue(Inline.FontStyleProperty, FontStyles.Italic);
            else
                contentRichTextBox.Selection.ApplyPropertyValue(Inline.FontStyleProperty, FontStyles.Normal);
        }
        private void fontFamilyComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(fontFamilyComboBox.SelectedItem != null)
            {
                contentRichTextBox.Selection.ApplyPropertyValue(Inline.FontFamilyProperty, fontFamilyComboBox.SelectedItem);
            }
        }
        private void fontSizeComboBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            contentRichTextBox.Selection.ApplyPropertyValue(Inline.FontSizeProperty, fontSizeComboBox.Text);
        }
        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {
            //tworzę nazwę mojego pliku
            string fileName = $"{viewModel.SelectedNote.Id}.rtf";
            //tworzę ścieżkę pliku
            string rtfFile = System.IO.Path.Combine(Environment.CurrentDirectory, fileName);

            //Tworzy się FileStream dla pliku RTF. Plik będzie utworzony (lub nadpisany, jeśli już istnieje) w trybie Create
            using (FileStream filestream = new FileStream(rtfFile, FileMode.Create))
            {
                //pobieram TextRanga między początkiem a końcem mojego EdytoraTekstu
                var content = new TextRange(contentRichTextBox.Document.ContentStart, contentRichTextBox.Document.ContentEnd);
                content.Save(filestream, DataFormats.Rtf);
            } ;
            //aktualizuje ścieżke notatki (właściwość obiektu notatka)
            viewModel.SelectedNote.FileLocation = await UpdateFile(rtfFile, fileName);
            //leci na firebase - serializuje się do JSONA i naura
            await DatabaseHelper.Update(viewModel.SelectedNote);
        }

        private async Task<string> UpdateFile(string rtfFile, string fileName)
        {
            //połączenie z Azure
            string connectionString = "*****************************************==;EndpointSuffix=core.windows.net";
            string containerName = "notes";

            var container = new BlobContainerClient(connectionString, containerName);
            // await container.CreateIfNotExistsAsync(); -> to niepotrzebne bo tworzę ten kontener przez platformę Azure

            var blob = container.GetBlobClient(fileName);
            //rtfFile to tak właściwie ścieżka do pliku :D
            await blob.UploadAsync(rtfFile);
            
            //ta metoda ma zwrócić lokalizacje pliku z notatkami, co w tym wypadku będzie
            //linkiem do mojego kontenera z Azura
            return $"https://nevernotestorage.blob.core.windows.net/notes/{fileName}";
        }
    }
}
