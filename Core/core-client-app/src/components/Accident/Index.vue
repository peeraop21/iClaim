<template>
    <div class="container">
        <div class="row">
            <div class="col-12" align="center">
                <h2 id="header2">ข้อมูลการรับแจ้งเหตุ</h2>
                <br>
                <div class="txt"  >
                    <p>ชื่อ-สกุล: {{userApi.prefix}}{{userApi.fname}} {{userApi.lname}}</p>
                    <p>เลขประจำตัวประชาชน: {{userApi.idcardNo}}</p>
                </div>
                <section>
                    <div style="height: 80%; width: 100%;">
                        <div class="accordion" v-for="accident in accidentsApi" v-bind:key="accident.eaTmpId">
                            <!--v-for="accident in accidents" :key="accident.id"-->
                            <div class="accordion-item" :id="'list' + accident.eaTmpId">
                                <a class="accordion-link" :href="'#list' + accident.eaTmpId">
                                    <div>
                                        <p>
                                            <ion-icon name="newspaper-outline"></ion-icon>เลขที่รับแจ้ง: {{ accident.eaAccNo }}
                                            <br>
                                            <ion-icon name="calendar-outline"></ion-icon>วันที่เกิดเหตุ: {{ accident.eaAccDate }}
                                        </p>
                                    </div>
                                    <ion-icon name="chevron-down-outline" class="icon ion-md-add"></ion-icon>
                                </a>
                                <div class="answer">
                                    <p>
                                        ทะเบียนรถ:
                                        <label v-for="car in accident.eaCar" v-bind:key="car.eaCarNo"> {{ car.eaCarLicense }},</label>
                                    </p>
                                    <p>
                                        สิทธิ์คงเหลือ:  บาท
                                    </p>
                                </div>
                                <div style="text-align: center">
                                    <button class="btn-select" @click="sendData">ใช้สิทธิ์</button>
                                    <router-link class="btn-checked" to="/CheckStatus">ติดตามสถานะ</router-link>
                                </div>
                            </div>
                        </div>
                    </div>
                </section>
            </div>
        </div>
        <br>
    </div>
</template>

<script>
    import axios from 'axios'
    export default {
        name: 'Accident',
        data() {
            return {
                userToken: "U616533ccb2a96fde1b9650e5181e768e",
                userApi: [],
                accidentsApi: [],               
            }
        },
        methods: {
            sendData() {
                this.$router.push({
                    name: "Claim",
                    query: { accData: this.accidentsApi }
                });
            },
            getAccidents() {
                var url = '/api/accident/{userToken}'.replace('{userToken}', this.userToken);
                axios.get(url)
                    .then((response) => {
                        this.accidentsApi = response.data;
                        console.log(this.accidentsApi);
                    })
                    .catch(function (error) {
                        alert(error);
                    });
            },
            getUser() {
                var url = '/api/user/{userToken}'.replace('{userToken}', this.userToken);
                axios.get(url)
                    .then((response) => {
                        this.userApi = response.data;
                        console.log(this.userApi);
                    })
                    .catch(function (error) {
                        alert(error);
                    });
            }
        },
        mounted() {
            this.getAccidents();
            this.getUser();
        }


    }
</script>

<style>
#header2 {
  font-size: 18px;
  font-weight: bold;
}
p.p_right{
    text-align: right; 
    width: 90%;
}
.txt {
  border-radius: 10px;
  border: 1px solid #cccccc;
  /*height: 300px; */
  padding: 25px 25px 15px 25px; 
  width: 100%;
  text-align: left;
  line-height: 10px;
}

    .mybtn {
        position: center;
        margin-left: 0px;
        justify-content: space-between;
        display: inline-block;
    }


    .btn-select {
        background-color: #5c2e91;
        margin-top: 10px;
        margin-right: 0px;
        width: 130px;
        color: white;
        padding: 3px 10px;
        text-align: center;
        text-decoration: none;
        display: inline-block;
        border-radius: 30px;
        border: none;
    }

    a[class="btn-checked"]:link, a[class="btn-checked"]:visited {
        background-color: #d775ff;
        margin-top: 10px;
        margin-left: 10px;
        color: white;
        padding: 3px 15px;
        text-align: center;
        text-decoration: none;
        display: inline-block;
        border-radius: 30px;
        /*font-size: 14px;*/
    }

    a[class="btn-checked"]:hover, a[class="btn-checked"]:active {
        background-color: #a55ac4;
    }

    section {
        width: 100%;
        height: 70%;
        display: flex;
        align-Items: center;
        justify-content: center;
        text-align: left;
    }

    .accordion-item {
        background-color: white;
        border-radius: .4rem;
        border-color: #cccccc;
        margin-bottom: .5rem;
        padding: 1rem 1rem .5rem 1rem;
        border-bottom: 3.5px solid #bbbbbb;
    }

    .accordion-link {
        color: black;
        background-color: white;
        width: 100%;
        display: flex;
        align-Items: center;
        justify-content: space-between;
        padding: 0;
        line-height: 25px;
        text-decoration: none;
    }

        .accordion-link:hover {
            text-decoration: none;
            color: #5c2e91;
        }

        .accordion-link ion-icon {
            color: #5c2e91;
            padding: 0rem .5rem .1rem 0rem;
            vertical-align: middle;
            font-size: 15px;
        }
        /*.accordion-link .ion-md-remove {
        display: none;
    }*/
        .accordion-link .ion-md-add {
            margin-bottom: 5px;
            font-size: 10px;
        }

    .answer {
        max-height: 0;
        overflow: hidden;
        position: relative;
        background-color: #eeeeee;
        transition: max-height 650ms;
    }

        .answer::before {
            content: "";
            position: absolute;
            width: .3rem;
            height: 100%;
            background-color: #5c2e91;
            top: 50%;
            left: 0;
            transform: translateY(-50%);
        }

        .answer p {
            color: black;
            padding: 0.7rem 0 0 2rem;
        }

    .accordion-item:target .answer {
        max-height: 15rem;
    }

    .accordion-item:target .accordion-link .ion-md-add {
        display: none;
    }

    .box-container {
        border-radius: 10px;
        border: 1px solid #cccccc;
        padding: 20px 20px 15px 20px;
        text-align: left;
    }
</style>
