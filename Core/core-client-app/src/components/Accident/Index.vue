<template>
    <div class="container">
        <div class="row">
            <div align="center">
                <h2 id="header2">ข้อมูลการรับแจ้งเหตุ</h2>
                <br>
                <!-- <div class="txt"  >
        <p>ชื่อ-สกุล: {{userData.prefix}}{{userData.fname}} {{userData.lname}}</p>
        <p>เลขประจำตัวประชาชน: {{userData.idcardNo}}</p>
    </div> -->
                <div class="tab-user">
                    <div class="row">
                        <div class="col-3 px-0">
                            <img src="@/assets/kid.png" width="70px">
                        </div>
                        <div class="col-9 text-start mt-2 px-0">
                            <span>ชื่อ-สกุล: {{userData.prefix}}{{userData.fname}} {{userData.lname}}</span><br>
                            <span>เลขประจำตัวประชาชน: {{userData.idcardNo}}</span>
                        </div>
                    </div>
                </div>
                <div class="mb-4 mt-4">
                    <router-link class="btn-next" to="/">แจ้งเหตุใหม่</router-link>
                </div>
                <div align="left" style="width: 100%;" >
                    <label class="title-advice-menu">ประวัติการแจ้งอุบัติเหตุ</label>
                </div>
                <section>
                    <div style="height: 98%; width: 100%;">
                        <div class="accordion" v-for="accident in accData" v-bind:key="accident.eaTmpId">
                            <!--v-for="accident in accidents" :key="accident.id"-->
                            <div class="accordion-item" :id="'list' + accident.eaTmpId">
                                <a class="accordion-link" :href="'#list' + accident.eaTmpId">
                                    <div>
                                        <p>
                                            <ion-icon name="newspaper-outline"></ion-icon>เลขที่รับแจ้ง: {{ accident.eaAccNo }}
                                            <br>
                                            <ion-icon name="calendar-outline"></ion-icon>วันที่เกิดเหตุ: {{ accident.stringAccDate }}
                                        </p>
                                    </div>
                                    <ion-icon name="chevron-down-outline" class="icon ion-md-add"></ion-icon>
                                </a>
                                <div class="answer">
                                    <p>
                                        ทะเบียนรถ:
                                        <label v-for="car in accident.eaCar" v-bind:key="car.eaCarNo"> {{ car.eaCarLicense }},</label>
                                    <br />
                                        สิทธิ์คงเหลือ:  บาท
                                    </p>
                                </div>
                                <div style="text-align: center">
                                    <!--<button class="btn-select" @click="sendData">ใช้สิทธิ์</button>-->
                                    <router-link class="btn-select"  :to="{ name: 'Rights', params: { id: accident.eaTmpId}}">ใช้สิทธิ์</router-link>
                                    <router-link class="btn-checked"  to="/CheckStatus">ติดตามสถานะ</router-link>

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
                userData: [],
                accData: [],               
            }
        },
        
        
        methods: {
            //senddata() {
            //    this.$router.push({
            //        name: "claim",
            //        query: { accdata: this.accidentsapi }
            //    });
            //},
            getAccidents() {
                var url = '/api/accident/{userToken}'.replace('{userToken}', this.userToken);
                axios.get(url)
                    .then((response) => {
                        /*this.accidentsApi = response.data;*/
                        this.$store.state.accStateData = response.data;
                        this.accData = this.$store.state.accStateData
                        console.log(this.$store.state.accStateData);
                    })
                    .catch(function (error) {
                        alert(error);
                    });
            },
            getUser() {
                var url = '/api/user/{userToken}'.replace('{userToken}', this.userToken);
                axios.get(url)
                    .then((response) => {
                        /*this.userApi = response.data;*/
                        this.$store.state.userStateData = response.data;
                        this.userData = this.$store.state.userStateData;
                        console.log(this.$store.state.userStateData);
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
    line-height: 15px;
}
    .tab-user {
        cursor: pointer;
        position: relative;
        padding: 1rem 1rem;
        border-radius: 1rem;
        line-height: 1.5rem;
        font-size: 12px;
        font-weight: 600;
        border: 1px solid #ddd;
        background-image: linear-gradient(-180deg, #FFFFFF 0%, #FFFFFF 50%);
        /*box-shadow: 0 0rem 0.5rem 0 #f5f5f5 inset, 0 -0.25rem 1.5rem #f5f5f5 inset, 0 0.75rem 0.5rem #f5f5f5 inset, 0 0.2rem 0.5rem 0 #f1f1f1 inset;*/
    }

    .tab-user.span {
        color: black;
        background-image: linear-gradient(0deg, #FFFFFF 0%, #FFFFFF 10%);
        -webkit-background-clip: text;
        background-clip: text;
    }

    /*.tab-user::before {
        content: "";
        display: block;
        height: 0.25rem;
        position: absolute;
        top: 0.5rem;
        left: 50%;
        transform: translateX(-50%);
        width: calc(100% - 7.5rem);
        background: #fff;
        border-radius: 100%;
        opacity: 0.8;
        background-image: linear-gradient(-270deg, #EEEEEE 0%, #FFFFFF 20%, #FFFFFF 80%, #EEEEEE 100%);
    }*/
.mybtn {
    position: center;
    margin-left: 0px;
    justify-content: space-between;
    display: inline-block;
}

    a[class="btn-next"]:link, a[class="btn-next"]:visited {
        background-color: var(--main-color);
        color: white;
        padding: 5px 50px;
        text-align: center;
        text-decoration: none;
        display: inline-block;
        border-radius: 10px;
        font-size: 15px;
    }

    .btn-select {
        background-color: var(--main-color);
        color: white;
        padding: 85px 90px;
        text-align: center;
        text-decoration: none;
        display: inline-block;
        border-radius: 10px;
        font-size: 15px;
    }
   
    .btn-next:link, .btn-next:visited {
        background-color: var(--main-color);
        color: white;
        padding: 5px 50px;
        text-align: center;
        text-decoration: none;
        display: inline-block;
        border-radius: 10px;
        font-size: 15px;
    }
    
    a[class="btn-select"]:link, a[class="btn-select"]:visited {
        background-color: var(--main-color);
        margin-top: 10px;
        margin-left: 10px;
        color: white;
        padding: 3px 35px;
        text-align: center;
        text-decoration: none;
        display: inline-block;
        border-radius: 10px;
        /*font-size: 14px;*/
    }


    a[class="btn-checked"]:link, a[class="btn-checked"]:visited {
        background-color: white;
        margin-top: 10px;
        margin-left: 10px;
        color: #5c2e91;
        padding: 3px 15px;
        text-align: center;
        text-decoration: none;
        display: inline-block;
        border-radius: 10px;
        border-style: solid;
        border-color: var(--main-color);
        font-weight: bold;
        /*font-size: 14px;*/
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
        color: var(--main-color);
        padding: 0rem .5rem .1rem 0rem;
        vertical-align: middle;
        font-size: 18px;
    }
    /*.accordion-link .ion-md-remove {
    display: none;
}*/
    .accordion-link .ion-md-add {
        margin-bottom: 5px;
        font-size: 16px;
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
        background-color: var(--main-color);
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
