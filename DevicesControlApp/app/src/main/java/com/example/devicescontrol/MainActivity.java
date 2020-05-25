package com.example.devicescontrol;

import androidx.annotation.NonNull;
import androidx.appcompat.app.AppCompatActivity;
import androidx.core.app.ActivityCompat;
import androidx.core.content.ContextCompat;

import android.Manifest;
import android.annotation.TargetApi;
import android.app.AlertDialog;
import android.app.ProgressDialog;
import android.content.DialogInterface;
import android.content.Intent;
import android.content.pm.PackageManager;
import android.os.AsyncTask;
import android.os.Build;
import android.os.Bundle;
import android.os.Handler;
import android.util.Log;
import android.view.View;
import android.widget.AdapterView;
import android.widget.ArrayAdapter;
import android.widget.ListView;
import android.widget.TextView;
import android.widget.Toast;

import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStream;
import java.io.InputStreamReader;
import java.net.HttpURLConnection;
import java.net.MalformedURLException;
import java.net.URL;

import static android.Manifest.permission.INTERNET;
import static android.Manifest.permission.ACCESS_NETWORK_STATE;
public class MainActivity extends AppCompatActivity implements AdapterView.OnItemClickListener {

    ListView lvRoom;
    String[] rooms;
    String[] roomsID;
    String roomList = "";
    ProgressDialog pd;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);

        askForPermission(Manifest.permission.INTERNET, 200);
        askForPermission(Manifest.permission.ACCESS_NETWORK_STATE, 200);

        //new JsonTask().execute("http://13.75.55.240/api/getRoomList");
        Handler handler = new Handler();
        handler.postDelayed(new Runnable() {
            @Override
            public void run() {
                new JsonTask().execute("http://13.75.55.240/api/getRoomList");

            }
        }, 2000);
    }

    public void getStringJson(String roomListed){
        try {
            JSONArray arr = new JSONArray(roomListed);
            System.out.println(roomListed);
            rooms = new String[arr.length()];
            roomsID = new String[arr.length()];
            for(int i=0;i<arr.length();i++){
                JSONObject jObj = arr.getJSONObject(i);
                String room = jObj.getString("roomName");
                String roomId = jObj.getString("roomId");
                rooms[i] = room;
                roomsID[i] = roomId;
                System.out.println(roomsID[i]);
            }

//            TextView testing = (TextView)findViewById(R.id.mainTitle) ;
//            testing.setText(rooms[0]);

            lvRoom = findViewById(R.id.lvRoom);
            ArrayAdapter<String> roomAdapter = new ArrayAdapter<>(this, android.R.layout.simple_list_item_1, rooms);
            lvRoom.setAdapter(roomAdapter);
            lvRoom.setOnItemClickListener(this);
        } catch (Exception e) {

            e.printStackTrace();
        }
    }

    @Override
    public void onItemClick(AdapterView<?> parent, View view, int position, long id) {
        String room = parent.getItemAtPosition(position).toString();
        //Toast.makeText(getApplicationContext(), "Click" + room, Toast.LENGTH_SHORT ).show();
        String roomId = roomsID[position];
        System.out.println(roomId);
        goToRoom(view, room, roomId);
    }

    public void goToRoom(View view, String roomName, String roomId){
        Intent Inroom = new Intent(MainActivity.this, RoomScreen.class);
        Inroom.putExtra("roomID", roomId);
        Inroom.putExtra("roomName", roomName);
        startActivity(new Intent(Inroom));
    }

//

    private class JsonTask extends AsyncTask<String, String, String> {

        protected void onPreExecute() {
            super.onPreExecute();

            pd = new ProgressDialog(MainActivity.this);
            pd.setMessage("Please wait");
            pd.setCancelable(false);
            pd.show();
        }

        protected String doInBackground(String... params) {


            HttpURLConnection connection = null;
            BufferedReader reader = null;

            try {
                URL url = new URL(params[0]);
                connection = (HttpURLConnection) url.openConnection();
                connection.connect();


                InputStream stream = connection.getInputStream();

                reader = new BufferedReader(new InputStreamReader(stream));

                StringBuffer buffer = new StringBuffer();
                String line = "";

                while ((line = reader.readLine()) != null) {
                    buffer.append(line+"\n");
                    Log.d("Response: ", "> " + line);   //here u ll get whole response...... :-)

                }

                return buffer.toString();


            } catch (MalformedURLException e) {
                e.printStackTrace();
            } catch (IOException e) {
                e.printStackTrace();
            } finally {
                if (connection != null) {
                    connection.disconnect();
                }
                try {
                    if (reader != null) {
                        reader.close();
                    }
                } catch (IOException e) {
                    e.printStackTrace();
                }
            }
            return null;
        }

        @Override
        protected void onPostExecute(String result) {
            super.onPostExecute(result);
            if (pd.isShowing()){
                pd.dismiss();
            }
            //System.out.println("inside" + result);
            getStringJson(result);
            roomList = result;
        }
    }

    private void askForPermission(String permission, Integer requestCode) {
        if (ContextCompat.checkSelfPermission(MainActivity.this, permission) != PackageManager.PERMISSION_GRANTED) {

            // Should we show an explanation?
            if (ActivityCompat.shouldShowRequestPermissionRationale(MainActivity.this, permission)) {

                //This is called if user has denied the permission before
                //In this case I am just asking the permission again
                ActivityCompat.requestPermissions(MainActivity.this, new String[]{permission}, requestCode);

            } else {

                ActivityCompat.requestPermissions(MainActivity.this, new String[]{permission}, requestCode);
            }
        } else {
            Toast.makeText(this, "" + permission + " is already granted.", Toast.LENGTH_SHORT).show();
        }
    }

}
