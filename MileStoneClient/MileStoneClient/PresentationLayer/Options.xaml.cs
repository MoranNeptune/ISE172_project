using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

//einat update - the bestest update ever

namespace MileStoneClient.PresentationLayer
{
    /// <summary>
    /// Interaction logic for Options.xaml
    /// </summary>
    public partial class Options : Page
    {
        private ObservableObject obs;
        private ChatRoomWindow chatRoom;
        private string filterChoice, sortChoice, orderChoice;
        private string prevFilter, prevSort, prevOrder;
        private bool isChanged;
        private List<string> users, groups;
        private string userChoice, groupChoice;
        private ComboBox userBox, groupBox;

        public Options(ChatRoomWindow cr, List<string> nicknames, List<string> groups, ObservableObject obs)
        {
            chatRoom = cr;
            this.obs = obs;
            DataContext = obs;
            InitializeComponent();
            isChanged = false;
            users = nicknames;
            this.groups = groups;
            userBox = new ComboBox();
            groupBox = new ComboBox();

            //initialize with ascending sort, none filter and sort by timestamp
            obs.AscendingIsChecked = true;
            obs.F_NoneIsChecked = true;
            obs.S_TimeIsChecked = true;
            filterChoice = "none";
            sortChoice = "time";
            orderChoice = "ascending";
            prevFilter = filterChoice;
            prevSort = sortChoice;
            prevOrder = orderChoice;
        }

        //sort by nickname
        private void SortNickname(object sender, RoutedEventArgs e)
        {
            obs.S_NicknameIsChecked = true;
            sortChoice = "name";
        }

        //sort by group id, nickname and timestemp 
        private void SortAll(object sender, RoutedEventArgs e)
        {
            obs.S_AllIsChecked = true;
            sortChoice = "all";
        }

        //sort by timestemp
        private void SortTime(object sender, RoutedEventArgs e)
        {
            obs.S_TimeIsChecked = true;
            sortChoice = "time";
        }

        //sort messages by ascending order
        private void SortAscending(object sender, RoutedEventArgs e)
        {
            obs.AscendingIsChecked = true;
            orderChoice = "ascending";
        }

        //sort messages by decending order 
        private void SortDescending(object sender, RoutedEventArgs e)
        {
            obs.DescendingIsChecked = true;
            orderChoice = "decending";
        }

        //no filter - if "None" filter is checked, hide the comboBox for users and groups
        private void FilterNone(object sender, RoutedEventArgs e)
        {
            obs.F_NoneIsChecked = true;
            filterChoice = "none";
            obs.IsUserFiltered = "Hidden";
            obs.IsGroupFiltered = "Hidden";
        }

        //filter by group id - if "Group Id" is checked, only display comboBox for groups
        private void FilterG_Id(object sender, RoutedEventArgs e)
        {
            obs.F_GroupIsChecked = true;
            filterChoice = "group";
            obs.IsUserFiltered = "Hidden";
            obs.IsGroupFiltered = "visible";
        }

        //filter by user name - if "User" filter is checked, show comboBox for groups
        private void FilterUser(object sender, RoutedEventArgs e)
        {
            obs.F_UserIsChecked = true;
            filterChoice = "user";
            obs.IsGroupFiltered = "visible";
        }

        //update isChanged status and hide the page
        private void btnOk(object sender, RoutedEventArgs e)
        {
            obs.IsOptionVisible = null;

            if (!orderChoice.Equals(prevOrder) | !filterChoice.Equals(prevFilter) | !sortChoice.Equals(prevSort))
            {
                isChanged = true;
            }
        }

        //initiate list for groups comboBox
        private void addGroups(object sender, RoutedEventArgs e)
        {
            var groupBox = sender as ComboBox;
            groupBox.ItemsSource = groups;
            groupBox.SelectedIndex = 0;
        }

        //display list for groups comboBox
        private void groupList(object sender, SelectionChangedEventArgs e)
        {
            var groupList = sender as ComboBox;
            string group = groupList.SelectedItem as string;
            groupChoice = group;

            if (groupList.SelectedIndex != 0)
            {
                obs.UserList.Clear();
                users = chatRoom.getMembersOf(groupChoice);
                // if there are no members in the group
                if (users.Count == 1)
                    obs.UserList.Add("Empty group :(");
                else
                {
                    // adds the groups members to the combobox
                    for (int i = 0; i < users.Count; i++)
                    {
                        obs.UserList.Add(users[i]);
                    }
                }
                obs.IsUserFiltered = "visible";
            }
        }


        //initiate list for users comboBox
        private void addUsers(object sender, RoutedEventArgs e)
        {
            var userBox = sender as ComboBox;
            userBox.ItemsSource = obs.UserList;
            userBox.SelectedIndex = 0;
        }

        //display list for users comboBox
        private void userList(object sender, SelectionChangedEventArgs e)
        {
            var userList = sender as ComboBox;
            string name = userList.SelectedItem as string;
            userChoice = name;
        }

        public bool IsChanged
        {
            get { return isChanged; }
            set { isChanged = value; }
        }

        public string FilterChoice
        {
            get { return filterChoice; }
        }

        public string SortChoice
        {
            get { return sortChoice; }
        }

        public string OrderChoice
        {
            get { return orderChoice; }
        }

        public string UserChoice
        {
            get { return userChoice; }
        }

        public string GroupChoice
        {
            get { return groupChoice; }
        }

    }
}
