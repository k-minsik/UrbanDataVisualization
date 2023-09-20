using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class InitializeData
{
    public static void InitializePlanetData() {
        List<string> data_names = new List<string> { "일별 교통량", "일별 대중교통 이용량", "따릉이 이용현황", "대기오염도", "일별 카드 결제건수", "골목상권 업소별 매출", "주요상권 일별 유동인구", "인천공항 출입 화물갯수" };
        List<string> metadata = new List<string> {
         "데이터 : 일별 교통량,단위 - 일별,색상 - 지점 코드 A~F,크기 - 교통량,경도 - 일자,위도 - 시간대,거리 - 지점,기간(코로나 이전) : 2019/6/1 ~ 2019/11/30,기간(코로나 이후) - 2020/7/1 ~ 2021/1/30,출처 - 서울 교통정보 센터"
        ,"데이터 : 일별 대중교통 이용량,단위 - 일별,색상 - 교통수단,크기 - 이용량,경도 - 일자,위도 - 시간대,거리 - 구,기간(코로나 이전) : 2019/6/1 ~ 2019/11/30,기간(코로나 이후) - 2020/7/1 ~ 2021/1/30,출처 - 교통카드 빅데이터 시스템"
        ,"데이터 : 따릉이 이용현황,단위 - 건수별,색상 - 이동거리,크기 - 사용시간,경도 - 일자,위도 - 시간대,거리 - ?,기간(코로나 이전) : 2019/6/1 ~ 2019/11/30,기간(코로나 이후) - 2020/7/1 ~ 2021/1/30,출처 - 서울시 열린 데이터 광장"
        ,"데이터 : 대기오염도,단위 - 일별&측정소별,색상 - 오염원 구분,크기 - 오염원양,경도 - 일자,위도 - ?,거리 - ?,기간(코로나 이전) : 2019년 전체,기간(코로나 이후) - 2020년 전체,출처 - 서울시 열린 데이터 광장"
        ,"데이터 : 일별 카드 결제건수,단위 - 일별,색상 - 업종,크기 - 매출액,경도 - 일자,위도 - ?,거리 - ?,기간(코로나 이전) : 2019/1/1 ~ 2019/6/30,기간(코로나 이후) - 2020/1/1 ~ 2021/6/30,출처 - 한국 데이터 거래소"
        ,"데이터 : 골목상권 업소별 매출,단위 - 상권별&업종별,색상 - 업종,크기 - 매출액,경도 - 일자,위도 - ?,거리 - ?,기간(코로나 이전) : 2019/1/1 ~ 2019/6/30,기간(코로나 이후) - 2020/1/1 ~ 2020/6/30,출처 - 서울시 우리 마을가게 상권 분석"
        ,"데이터 : 주요상권 일별 유동인구,단위 - 일별&상권별,색상 - 상권,크기 - 유동인구수,경도 - 일자,위도 - ?,거리 - ?,기간(코로나 이전) : 2019/12/1 ~ 2019/12/31,기간(코로나 이후) - 2020/1/1 ~ 2020/2/8,출처 - 한국 데이터 거래소"
        ,"데이터 : 인천공항 출입 화물갯수,단위 - 일별,색상 - 구분,크기 - 운항수,경도 - 일자,위도 - 시간대,거리 - 지점,기간(코로나 이전) : 2019/1/1 ~ 2019/3/31,기간(코로나 이후) - 2021/1/1 ~ 2021/3/31,출처 - 인천공항" };
        
        for ( int planetNo = 0; planetNo < data_names.Count; planetNo++) {
            // Make new List<Balloon> for Planet
            List<Balloon> temp_balloonlist = new List<Balloon>();
            List<Balloon> temp_balloonlist_AfterCovid = new List<Balloon>();

            // Load Planet's Mesh from Prefab with data_name
            GameObject temp_planetMesh = Resources.Load("Prefabs/Planet_" + data_names[planetNo]) as GameObject;

            // If Balloon's mesh does not differ by planet, Option 1 can be used outside this foreach loop
            // [Opt1] GameObject temp_balloonMesh = Resources.Load("Prefabs/Balloon") as GameObject;
            // [Opt2] GameObject temp_balloonMesh = Resources.Load("Prefabs/Balloon_" + data_names[i]) as GameObject;
            GameObject temp_balloonMesh = Resources.Load("Prefabs/Balloon") as GameObject;

            // Calculate each Planet's position with Number of Planets (data_names.Count)
            const float radius = 10.0f;
            float angle = (2 * Mathf.PI * planetNo) / data_names.Count;
            float x = Mathf.Sin(angle) * radius;
            float z = Mathf.Cos(angle) * radius;

            Vector3 temp_planetPosition = new Vector3(x,0f,z);
            //Debug.Log(temp_planetPosition);
            
            // open csv file with data_names[i] and attach it to Stringreader
            TextAsset sourcefile = Resources.Load<TextAsset>(data_names[planetNo]);
            StringReader sr = new StringReader(sourcefile.text);
            
            string line;
            bool endOfFile = false;

            line = sr.ReadLine(); // Pass Header row

            // then, use stringreader to read each line
            while (!endOfFile) {
                // Read line by line
                line = sr.ReadLine();

                if( line==null ){
                    endOfFile = true;
                    break;
                }

                Balloon temp_balloon = InitializeBalloonData(line, temp_balloonMesh, data_names[planetNo]);
                
                // if balloon is before covid
                if (temp_balloon.IsBeforeCovid == true) {
                    temp_balloonlist.Add(temp_balloon);
                }
                // if balloon is after covid
                else {
                    temp_balloonlist_AfterCovid.Add(temp_balloon);
                }
            }

            // Create planet
            Planet temp_planet = PlanetManager.Instance.CreatePlanet( data_names[planetNo], temp_planetPosition, temp_planetMesh, temp_balloonlist);
            temp_planet.SetPlanetMetaData(metadata[planetNo].Split(','));
            PlanetManager.Instance.AddPlanetToMap(temp_planet);

            Planet temp_planet_AfterCovid = PlanetManager.Instance.CreatePlanet( data_names[planetNo] + "AfterCovid", temp_planetPosition, temp_planetMesh, temp_balloonlist_AfterCovid);
            //Debug.Log(temp_planet_AfterCovid.PlanetName);
            PlanetManager.Instance.AddPlanet_AfterCovidToMap(temp_planet_AfterCovid);
        }
    }

    private static Balloon InitializeBalloonData(string line, GameObject balloonMesh, string planetName) {
        // split line with ','
        string[] data_array = line.Split(',');

        float BalloonSize = 0.5f * 0.003f;
        float BalloonJitter = 4f;

        bool isBeforeCovid = true;

        float latitude = 0.0f;
        float longitude = 0.0f;
        float diameter = 0.0f;
        float radius = 0.0f;
        int color_idx = 0;

        switch (planetName) {
            case "일별 교통량":  // data structure : [(일자), (요일), (지점번호), (해당 지점 평균 통행량), (합계), (0시), (1시), ... (23시), (0시평균) ,(1시평균), ...(23시 평균)]
                string date = data_array[0];
                float day = (date[6] - '0') * 10 + (date[7] - '0');
                string location = data_array[2];
                float average_traffic = float.Parse(data_array[3]);

                int traffic_total = 0;
                float max_traffic_rate = 0;
                float min_traffic_rate = 0;
                int busiest_hour = 0;

                for (int i = 5; i < 29; i++)
                {
                    int traffic_value = int.Parse(data_array[i]);
                    float traffic_average = float.Parse(data_array[i + 24]);
                    traffic_total += traffic_value;

                    float increase_rate = (traffic_value - traffic_average) / traffic_average;

                    if (increase_rate > max_traffic_rate)
                    {
                        max_traffic_rate = increase_rate;
                        busiest_hour = i - 5;
                    }
                    if (increase_rate < min_traffic_rate)
                    {
                        min_traffic_rate = increase_rate;
                    }

                }

                float traffic_rate = (float)((traffic_total - average_traffic)) / average_traffic;

                isBeforeCovid = int.Parse(date) < 20200000;

                float maxAbs_traffic_rate = Mathf.Abs(max_traffic_rate) > Mathf.Abs(min_traffic_rate) ? max_traffic_rate : min_traffic_rate;
                radius = Mathf.Pow(BalloonJitter,  maxAbs_traffic_rate) * BalloonSize;

                longitude = 360 * (float)day / 31 + Random.Range(0f, 12f);
                //float latitude = 180 * (float)busiest_hour / 24 + Random.Range(0f, 7.5f);
                latitude = maxAbs_traffic_rate * 90 ;
                //float diameter = PlanetSize * ((location[0] - 'A') / 6 + 1);
                diameter = Mathf.Pow(4f, traffic_rate) * 100f;
                color_idx = busiest_hour;


                break;
            case "일별 대중교통 이용량":
                break;
            case "따릉이 이용현황":
                break;
            case "대기오염도":
                break;
            case "일별 카드 결제건수":
                break;
        }

        float x = diameter * Mathf.Sin(latitude) * Mathf.Cos(longitude);
        float y = diameter * Mathf.Cos(latitude);
        float z = diameter * Mathf.Sin(latitude) * Mathf.Sin(longitude);        

        Vector3 temp_balloonPosition = new Vector3(x,y,z) * 0.0045f;
        float temp_sizeOffset = radius;
        Balloon temp_balloon = BalloonManager.Instance.CreateBalloon(temp_balloonPosition,temp_sizeOffset,balloonMesh, color_idx, isBeforeCovid);
        BalloonManager.Instance.AddBalloonToMap(temp_balloon);

        return temp_balloon;
    }
    
}
