package com.example.devicescontrol;

import android.annotation.SuppressLint;
import android.app.AlertDialog;
import android.app.Dialog;
import android.app.ProgressDialog;
import android.content.DialogInterface;
import android.content.Intent;
import android.os.AsyncTask;
import android.os.Bundle;
import android.os.Handler;
import android.text.InputType;
import android.util.Log;
import android.view.View;
import android.widget.AdapterView;
import android.widget.ArrayAdapter;
import android.widget.Button;
import android.widget.EditText;
import android.widget.ListView;
import android.widget.NumberPicker;
import android.widget.Switch;
import android.widget.TextView;
import android.widget.Toast;
import android.widget.ToggleButton;

import androidx.appcompat.app.AppCompatActivity;

import org.apache.http.client.methods.HttpPost;
import org.apache.http.entity.StringEntity;
import org.apache.http.impl.client.BasicResponseHandler;
import org.apache.http.impl.client.DefaultHttpClient;
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
import java.util.ArrayList;
import java.util.List;

public class RoomScreen extends AppCompatActivity implements AdapterView.OnItemClickListener, NumberPicker.OnValueChangeListener {
    TextView tvTop;
    ListView lvDevice;
    static Dialog d;
    String tempSet = "25.5";
    String[] devices;
    String[] status;
    String[] setValue;
    List<String> data;
    List<String> detail;
    List<String> statusList;
    ProgressDialog pd;
    String deviceList;
    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.room_screen);
        tvTop = (TextView) findViewById(R.id.RoomTitle);
        Intent intent = getIntent();
        String roomID = intent.getStringExtra("roomID");
        String roomName = intent.getStringExtra("roomName");
        tvTop.setText("Room: " + roomName);


        String url = "http://13.75.55.240/api/getDeviceListByRoomId/" + roomID;
        System.out.println(url);
        new RoomScreen.JsonTask().execute(url);
        Handler handler = new Handler();
        handler.postDelayed(new Runnable() {
            @Override
            public void run() {
                getStringJson();
            }
        }, 3000);
    }

    public void getStringJson(){
        try {
            JSONArray arr = new JSONArray(deviceList);
            devices = new String[arr.length()];
            status = new String[arr.length()];
            setValue = new String[arr.length()];
            for(int i=0;i<arr.length();i++){
                JSONObject jObj = arr.getJSONObject(i);
                String deviceId = jObj.getString("deviceId");
                String Status = jObj.getString("status");
                String SetValue = jObj.getString("set_value");
                devices[i] = deviceId;
                status[i] = Status;
                setValue[i] = SetValue;
                //System.out.println(roomsID[i]);
            }

            data = new ArrayList<>();
            detail = new ArrayList<>();
            statusList = new ArrayList<>();
            lvDevice = findViewById(R.id.deviceList);
            for (int i = 0; i < arr.length(); i++) {
                if(devices[i].contains("AC")){
                    data.add(devices[i]);///////////////////////////
                    detail.add(setValue[i]);
                }else{
                    data.add(devices[i]);///////////////////////////
                    detail.add("");
                }

                if(status[i].equals("True")){
                    statusList.add("true");
                }else{
                    statusList.add("false");
                }

            }
            MyAdapter roomAdapter = new MyAdapter(data, detail, statusList);
            //ArrayAdapter<String> roomAdapter = new ArrayAdapter<>(this, android.R.layout.simple_list_item_1, devices);
            lvDevice.setAdapter(roomAdapter);
            lvDevice.setOnItemClickListener(this);

        } catch (JSONException e) {
            e.printStackTrace();
        }
    }

    @Override
    public void onItemClick(AdapterView<?> parent, View view, int position, long id) {
        String device = parent.getItemAtPosition(position).toString();
        if (device.contains("AC")) {
            popupDialog(device, setValue[position]);
        }
    }

    public void popupDialog(final String device, String value) {
        final Dialog d = new Dialog(RoomScreen.this);
        String[] temperatures = {"16", "16.5", "17", "17.5", "18.5", "19", "19.5", "20", "20.5", "21", "21.5", "22", "22.5", "23", "23.5", "24", "24.5", "25", "25.5", "26", "26.5", "27", "27.5", "28", "28.5", "29", "29.5", "30"};
        d.setTitle("NumberPicker");
        d.setContentView(R.layout.ac_dialog);
        Button b1 = (Button) d.findViewById(R.id.button1);
        Button b2 = (Button) d.findViewById(R.id.button2);
        final NumberPicker np = (NumberPicker) d.findViewById(R.id.numberPicker1);
        np.setMaxValue(28);
        np.setMinValue(1);
        np.setWrapSelectorWheel(false);
        np.setDisplayedValues(temperatures);
        np.setValue(setValueConvent(value));
        np.setOnValueChangedListener(this);
        b1.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                //tv.setText(String.valueOf(np.getValue())); //set the value to textview
                Toast.makeText(getApplicationContext(), "Temperature Set: " + setNumberConvent(String.valueOf(np.getValue())), Toast.LENGTH_LONG).show();
                tempSet = String.valueOf(np.getValue());
                detail.set(data.indexOf(device),tempSet);
                tempSet = setNumberConvent(tempSet);
                String apiData = "[{" +
                        '"' + "deviceID" + '"' + ":" + '"' + device + '"' + "," +
                        '"' + "status" + '"' + ":" + '"' + "true" + '"' + "," +
                        '"' + "set_value" + '"' + ":" + '"' + tempSet + '"' +
                        "}]";
                System.out.println("this:" + apiData);
                callApi(apiData);
                d.dismiss();
                finish();
                startActivity(getIntent());
            }
        });
        b2.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                d.dismiss(); // dismiss the dialog
            }
        });
        d.show();
    }

    public int setValueConvent(String value){
        int number= 1;
        switch (value){
            case"16":
                number = 1;
                break;
            case"16.5":
                number = 2;
                break;
            case"17":
                number = 3;
                break;
            case"17.5":
                number = 4;
                break;
            case"18.5":
                number = 5;
                break;
            case"19":
                number = 6;
                break;
            case"19.5":
                number = 7;
                break;
            case"20":
                number = 8;
                break;
            case"20.5":
                number = 9;
                break;
            case"21":
                number = 10;
                break;
            case"21.5":
                number = 11;
                break;
            case"22":
                number = 12;
                break;
            case"22.5":
                number = 13;
                break;
            case"23":
                number = 14;
                break;
            case"23.5":
                number = 15;
                break;
            case"24":
                number = 16;
                break;
            case"24.5":
                number = 17;
                break;
            case"25":
                number = 18;
                break;
            case"25.5":
                number = 19;
                break;
            case"26":
                number = 20;
                break;
            case"26.5":
                number = 21;
                break;
            case"27":
                number = 22;
                break;
            case"27.5":
                number = 23;
                break;
            case"28":
                number = 24;
                break;
            case"28.5":
                number = 25;
                break;
            case"29":
                number = 26;
                break;
            case"29.5":
                number = 27;
                break;
            case"30":
                number = 28;
                break;
        }
        return number;
    }

    public String setNumberConvent(String value){
        String number= "1";
        switch (value){
            case"1":
                number = "16";
                break;
            case"2":
                number = "16.5";
                break;
            case"3":
                number = "17";
                break;
            case"4":
                number = "17.5";
                break;
            case"5":
                number = "18.5";
                break;
            case"6":
                number = "19";
                break;
            case"7":
                number = "19.5";
                break;
            case"8":
                number = "20";
                break;
            case"9":
                number = "20.5";
                break;
            case"10":
                number = "21";
                break;
            case"11":
                number = "21.5";
                break;
            case"12":
                number = "22";
                break;
            case"13":
                number = "22.5";
                break;
            case"14":
                number = "23";
                break;
            case"15":
                number = "23.5";
                break;
            case"16":
                number = "24";
                break;
            case"17":
                number = "24.5";
                break;
            case"18":
                number = "25";
                break;
            case"19":
                number = "25.5";
                break;
            case"20":
                number = "26";
                break;
            case"21":
                number = "26.5";
                break;
            case"22":
                number = "27";
                break;
            case"23":
                number = "27.5";
                break;
            case"24":
                number = "28";
                break;
            case"25":
                number = "28.5";
                break;
            case"26":
                number = "29";
                break;
            case"27":
                number = "29.5";
                break;
            case"38":
                number = "30";
                break;
        }
        return number;
    }

    @Override
    public void onValueChange(NumberPicker picker, int oldVal, int newVal) {
        Log.i("value is", "" + newVal);
    }

    @SuppressLint("StaticFieldLeak")
    public void callApi(String data) {
        final String F_data = data;

        new AsyncTask<Void, Void, String>() {

            @Override
            protected String doInBackground(Void... voids) {
                return getServerResponse(F_data);
            }

            @Override
            protected void onPostExecute(String res) {

                //Toast.makeText(getApplicationContext(), "Server return: " + res, Toast.LENGTH_LONG).show();
            }

        }.execute();

    }

    public String getServerResponse(String data) {
        HttpPost post = new HttpPost("http://13.75.55.240/API/DeviceOnOffAPI");
        try {
            StringEntity entity = new StringEntity(data);
            post.setEntity(entity);
            post.setHeader("Content-type", "application/json");

            DefaultHttpClient client = new DefaultHttpClient();

            BasicResponseHandler handler = new BasicResponseHandler();
            String response = client.execute(post, handler);
            return response;
        } catch (IOException e) {
            Log.d("JMP", e.toString());
        }

        return "Can't connect server";
    }


    //////////////////////////////////////////////////////////
    private class JsonTask extends AsyncTask<String, String, String> {

        protected void onPreExecute() {
            super.onPreExecute();

            pd = new ProgressDialog(RoomScreen.this);
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
                    buffer.append(line + "\n");
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
            if (pd.isShowing()) {
                pd.dismiss();
            }
            //System.out.println("inside" + result);
            deviceList = result;
        }
    }

    public void finish(View view) {
        finish();
    }
}



