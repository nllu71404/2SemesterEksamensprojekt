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
        public readonly TopicRepository _topicRepository;
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

        public TopicPageViewModel(TopicRepository topicRepository)
        {
            _topicRepository = topicRepository;

            foreach (var topic in _topicRepository.GetAllTopics())
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
                MessageBox.Show("Udfyld venligst emnebeskrivelse");
                return;
            }

            var newTopic = new Topic
            {
                TopicDescription = TopicDescription,
            };

            int newTopicId = _topicRepository.SaveNewTopic(newTopic);
            newTopic.TopicId = newTopicId;

            Topics.Add(newTopic);
            TopicDescription = "";
            MessageBox.Show("Emne oprettet");
        }
        public void DeleteSelectedTopic()
        {
            if (SelectedTopic == null) return;

            var result = MessageBox.Show(
                $"Er du sikker på, at du vil slette emne: '{SelectedTopic.TopicDescription}'?",
                "Bekræft sletning",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning
            );

            if (result == MessageBoxResult.Yes)
            {
                _topicRepository.DeleteTopic(SelectedTopic.TopicId);
                Topics.Remove(SelectedTopic);

                SelectedTopic = null;
            }
        }

        public void EditSelectedTopic()
        {
            if(SelectedTopic == null)
            {
                MessageBox.Show("Vælg emne du vil redigerer.");
                return;
            }
            TopicDescription = SelectedTopic.TopicDescription;
        }

        public void SaveSelectedTopic()
        {
            if(SelectedTopic == null)
            {
                MessageBox.Show("Vælg emne du vil gemme.");
                return;
            }
            if (string.IsNullOrWhiteSpace(TopicDescription))
            {
                MessageBox.Show("Skriv en emnebeskrivelse");
                return;
            }
            
            SelectedTopic.TopicDescription = TopicDescription!;

            _topicRepository.UpdateTopic(SelectedTopic);

            //Reload liste
            Topics.Clear();
            foreach (var t in _topicRepository.GetAllTopics())
                Topics.Add(t);

            TopicDescription = string.Empty;


        }


    }
}
