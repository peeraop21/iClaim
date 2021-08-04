<template>
    <div class="container">
        <div class="row">
            <!--<div class="col-12">
                <br><br>
                <a class="btn-intro" href="default.asp">สิทธิ์คุ้มครองตาม พ.ร.บ.</a>
            </div>-->
            <div class="col-12">
                <div class="card card-advice-menu" data-bs-toggle="modal" data-bs-target="#rightsModal">
                    <div class="card-body">
                        <p class="title-advice-menu">สิทธิ์คุ้มครองตาม พ.ร.บ.</p>
                    </div>

                </div>
            </div>
        </div>
        <br>
        <div class="row">
            <div class="col-12">
                <div class="card card-advice-menu" data-bs-toggle="modal" data-bs-target="#procedureModal">
                    <div class="card-body">
                        <p class="title-advice-menu">
                            ขั้นตอนดำเนินการ
                            <br>และเอกสารที่ต้องเตรียม
                        </p>
                    </div>

                </div>
            </div>

            <!-- <div class="col-12">
                 <br><br>
                 <a class="btn-intro" href="default.asp">ขั้นตอนดำเนินการ<br>และเอกสารที่ต้องเตรียม</a>
             </div>-->
        </div>
        <br>
        <div class="row">
            <div class="col-12">
                <br>
                <br>
                <a class="btn-next" href="\Accident">{{ msg }}</a>

            </div>
        </div>
    </div>


  
</template>
<script>
    

    export default {
        name: 'Advice',
        data() {
            return {
                msg: "ดำเนินการต่อ"
            }
        }
        
    }
</script>
<style>
    .purple-title {
        color: #5c2e91;
        font-size: 20px;
    }

    .space-title {
        margin: 1.0rem 0rem;
    }

    .space-title-procedure {
        margin: 0rem 0rem 0.1rem 0rem;
    }

    .card-advice-menu {
        border: 2px solid #5c2e91;
        padding: 7% 0px;
        width: 70%;
        margin-top: 2rem;
        border-radius: 10px;
        text-align: center;
        text-decoration: none;
        display: inline-block;
    }

    .title-advice-menu {
        font-size: 1.5rem;
        color: black;
        margin-bottom: 0px;
    }

    a[class="btn-intro"]:link, a[class="btn-intro"]:visited {
        background-color: white;
        color: black;
        border: 2px solid #5c2e91;
        padding: 7% 20px;
        width: 70%;
        text-align: center;
        text-decoration: none;
        display: inline-block;
        border-radius: 10px;
        font-size: 20px;
    }

    a[class="btn-intro"]:hover, a[class="btn-intro"]:active {
        background-color: #5c2e91;
        color: white;
    }

    a[class="btn-next"]:link, a[class="btn-next"]:visited {
        background-color: #5c2e91;
        color: white;
        padding: 5px 50px;
        text-align: center;
        text-decoration: none;
        display: inline-block;
        border-radius: 30px;
        font-size: 20px;
    }

    a[class="btn-next"]:hover, a[class="btn-next"]:active {
        background-color: #50287e;
    }
</style>