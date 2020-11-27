using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;



namespace Application
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity, BottomNavigationView.IOnNavigationItemSelectedListener
    {
        EditText txtCode;
        TextView txtError;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_main);
            Compiler.LexicalAnalyst.setKeysSymbolTable();

            txtCode = FindViewById<EditText>(Resource.Id.code);
            txtError = FindViewById<TextView>(Resource.Id.error);

            BottomNavigationView navigation = FindViewById<BottomNavigationView>(Resource.Id.navigation);
            navigation.SetOnNavigationItemSelectedListener(this);
        }
        public bool OnNavigationItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.navigation_lexical:
                    txtError.Text = Compiler.LexicalAnalyst.Compile(txtCode.Text);
                    return true;
                case Resource.Id.navigation_syntax:
                    txtError.Text += Compiler.SyntacticAnalyst.Compile();
                    return true;
                case Resource.Id.navigation_semantic:
                    txtError.Text += Compiler.SemanticAnalyst.Compile();
                    return true;
            }
            return false;
        }
    }
}

