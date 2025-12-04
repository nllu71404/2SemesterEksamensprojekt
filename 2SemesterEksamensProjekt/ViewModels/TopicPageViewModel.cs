using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using _2SemesterEksamensProjekt.Commands;
using _2SemesterEksamensProjekt.Models;
using _2SemesterEksamensProjekt.Repository;
using _2SemesterEksamensProjekt.Views.Pages;

namespace _2SemesterEksamensProjekt.ViewModels
{
    public class TopicPageViewModel : BaseViewModel
    {
        public readonly ITopicRepository _topicRepo;
        public ObservableCollection<Topic> Topics { get; } = new ObservableCollection<Topic>();

        // Command properties
        public RelayCommand CreateTopicCommand { get; }
        public RelayCommand DeleteTopicCommand { get; }
        public RelayCommand EditSelectedTopicCommand { get; }
        public RelayCommand SaveSelectedTopicCommand { get; }


        // Inputs til topics
        private string _topicDescription;
        public string TopicDescription
        {
            get => _topicDescription;
            set => SetProperty(ref _topicDescription, value);
        }
        private Topic? _selectedTopic;
        public Topic? SelectedTopic
        {
            get => _selectedTopic;
            set => SetProperty(ref _selectedTopic, value);
        }

        public TopicPageViewModel(ITopicRepository topicRepo)
        {

            foreach (var topic in topicRepo.GetAllTopics())
            {
                Topics.Add(topic);
            }
            SelectedTopic = Topics.FirstOrDefault();

            // Commands initialiseres og kalder de eksisterende metoders
            CreateTopicCommand = new RelayCommand(_ => CreateTopic());
            DeleteTopicCommand = new RelayCommand(_ => DeleteSelectedTopic());
            EditSelectedTopicCommand = new RelayCommand(_ => EditSelectedTopic());
            SaveSelectedTopicCommand = new RelayCommand(_ => SaveSelectedTopic());
        }

        public void CreateTopic()
        {
            if (string.IsNullOrWhiteSpace(TopicDescription))
            {
                ShowMessage("Udfyld venligst emnebeskrivelse");
                return;
            }

            var newTopic = new Topic
            {
                TopicDescription = TopicDescription,
            };

            int newTopicId = _topicRepo.SaveNewTopic(newTopic);
            newTopic.TopicId = newTopicId;

            Topics.Add(newTopic);
            TopicDescription = "";
            ShowMessage("Emne oprettet");
        }
        public void DeleteSelectedTopic()
        {
            if (SelectedTopic == null) return;

            var result = ShowConfirmation(
                $"Er du sikker på, at du vil slette emne: '{SelectedTopic.TopicDescription}'?"
            );

            if (result == MessageBoxResult.Yes)
            {
                _topicRepo.DeleteTopic(SelectedTopic.TopicId);
                Topics.Remove(SelectedTopic);

                SelectedTopic = null;
            }
        }

        public void EditSelectedTopic()
        {
            if (SelectedTopic == null)
            {
                ShowMessage("Vælg emne du vil redigerer.");
                return;
            }
            TopicDescription = SelectedTopic.TopicDescription;
        }

        public void SaveSelectedTopic()
        {
            if (SelectedTopic == null)
            {
                ShowMessage("Vælg emne du vil gemme.");
                return;
            }
            if (string.IsNullOrWhiteSpace(TopicDescription))
            {
                ShowMessage("Skriv en emnebeskrivelse");
                return;
            }

            SelectedTopic.TopicDescription = TopicDescription!;

            _topicRepo.UpdateTopic(SelectedTopic);

            //Reload liste
            Topics.Clear();
            foreach (var t in _topicRepo.GetAllTopics())
                Topics.Add(t);

            TopicDescription = string.Empty;


        }

        // TEST: Override metode for at vise beskeder (kan tilpasses i tests)
        protected virtual void ShowMessage(string msg)
        {
            MessageBox.Show(msg);
        }

        // TEST: Override metode for bekræftelses-popup (kan tilpasses i tests)
        protected virtual MessageBoxResult ShowConfirmation(string message)
        {
            return MessageBox.Show(
                message,
                "Bekræft sletning",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning
            );
        }

    }
}
