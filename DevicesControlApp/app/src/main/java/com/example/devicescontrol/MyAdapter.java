package com.example.devicescontrol;

import android.annotation.SuppressLint;
import android.content.Context;
import android.os.AsyncTask;
import android.util.Log;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.BaseAdapter;
import android.widget.Button;
import android.widget.TextView;
import android.widget.Toast;
import android.widget.ToggleButton;

import org.apache.http.client.methods.HttpPost;
import org.apache.http.entity.StringEntity;
import org.apache.http.impl.client.BasicResponseHandler;
import org.apache.http.impl.client.DefaultHttpClient;

import java.io.IOException;
import java.util.List;

public class MyAdapter extends BaseAdapter implements View.OnClickListener {
    private Context context;
    private List<String> data;
    private List<String> detail;
    private List<String> status;
    public MyAdapter(List<String> data, List<String> detail, List<String> status){
        this.data = data;
        this.detail = detail;
        this.status = status;
    }
    @Override
    public int getCount() {
        return data == null ? 0 : data.size();
    }
    @Override
    public Object getItem(int i) {
        return data.get(i);
    }

    @Override
    public long getItemId(int i) {
        return i;
    }

    @Override
    public View getView(int i, View view, ViewGroup viewGroup) {
        ViewHolder viewHolder = null;
        if(context == null)
            context = viewGroup.getContext();
        if(view == null){
            view = LayoutInflater.from(viewGroup.getContext()).inflate(R.layout.list_toggle,null);
            viewHolder = new ViewHolder();
            viewHolder.mTv = (TextView)view.findViewById(R.id.mTv);
            viewHolder.mBtn = (ToggleButton)view.findViewById(R.id.mBtn);
            view.setTag(viewHolder);
        }
        viewHolder = (ViewHolder)view.getTag();
        viewHolder.mBtn.setTag(R.id.btn,i);
        viewHolder.mBtn.setOnClickListener(this);
        //System.out.println(i);
        if(status.get(i).equals("true")){
            viewHolder.mBtn.setChecked(true);
        }else{

            viewHolder.mBtn.setChecked(false);
        }
        String checkedDetail = "";
        if(detail.get(i).equals("")){
            checkedDetail = detail.get(i);
        }else{
            checkedDetail = ": " + detail.get(i) + "C";
        }
        viewHolder.mTv.setText(data.get(i) + checkedDetail);
        viewHolder.mTv.setTag(R.id.tv,i);
        viewHolder.mTv.setOnClickListener(this);
        return view;
    }
    @Override
    public void onClick(View view) {
        switch (view.getId()){
            case R.id.mBtn:
                int b = (int) view.getTag(R.id.btn);
                    //System.out.println(data.get(b));
                if(status.get(data.indexOf(data.get(b))).equals("true")){
                    status.set(data.indexOf(data.get(b)),"false");
                }else{
                    status.set(data.indexOf(data.get(b)),"true");
                }
                String apiData = "";
                if(status.get(data.indexOf(data.get(b))).equals("true")){
                    apiData = "[{" +
                            '"' + "deviceID" + '"' + ":" + '"' + data.get(b) + '"' + "," +
                            '"' + "status" + '"' + ":" + '"' + "true" +'"' + "," +
                            '"' + "set_value" + '"' + ":" + '"' + detail.get(data.indexOf(data.get(b))) + '"' +
                            "}]";
                }else{
                    apiData = "[{" +
                            '"' + "deviceID" + '"' + ":" + '"' + data.get(b) + '"' + "," +
                            '"' + "status" + '"' + ":" + '"' + "false" +'"' + "," +
                            '"' + "set_value" + '"' + ":" + '"' + detail.get(data.indexOf(data.get(b))) + '"' +
                            "}]";
                }


                System.out.println("adapt"  + apiData);
                    callApi(apiData);
                break;
            case R.id.mTv:
                int t = (int)view.getTag(R.id.tv);
                break;
        }
    }
    static class ViewHolder{
        TextView mTv;
        ToggleButton mBtn;
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
}
